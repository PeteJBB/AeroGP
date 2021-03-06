﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class Utility
{
	public static Vector3 WorldToGUIPoint(Vector3 position)
	{
		var pos = Camera.main.WorldToScreenPoint(position);
		pos.y = Screen.height - pos.y;
		if(pos.z < 0)
			pos *= -1;
		return pos;
	}

	public static Vector2 ScreenCenter()
	{
		return new Vector2(Screen.width / 2f, Screen.height / 2f);
	}

	public static void DrawRotatedGuiTexture(Rect rect, float angle, Texture texture)
	{
		Matrix4x4 matrixBackup = GUI.matrix;
		GUIUtility.RotateAroundPivot(angle, rect.center);
		GUI.DrawTexture(rect, texture);
		GUI.matrix = matrixBackup;
	}

    public static void DrawRotatedTextField(Rect rect, string text, float angle, GUIStyle style)
	{
		Matrix4x4 matrixBackup = GUI.matrix;
		GUIUtility.RotateAroundPivot(angle, rect.center);
        GUI.TextField(rect, text, style);
		GUI.matrix = matrixBackup;
	}

	public static void DrawRotatedGuiTexture(Rect screenRect, float angle, Texture texture, Rect srcRect, Material mat)
	{
		if (Event.current.type == EventType.Repaint)
		{
			Matrix4x4 matrixBackup = GUI.matrix;
			GUIUtility.RotateAroundPivot(angle, screenRect.center);
			Graphics.DrawTexture(screenRect, texture, srcRect, 0, 0, 0, 0, mat);
			GUI.matrix = matrixBackup;
		}
	}

    public static void DrawRectOutline(Rect rect, Color c, int lineWidth)
    {
        var rLeft = new Rect(rect.xMin, rect.yMin, lineWidth, rect.height);
        GUI.DrawTexture(rLeft, CreatePlainColorTexture(Color.white));

        var rRight = new Rect(rect.xMax - lineWidth, rect.yMin, lineWidth, rect.height);
        GUI.DrawTexture(rRight, CreatePlainColorTexture(Color.white));

        var rTop = new Rect(rect.xMin, rect.yMin, rect.width, lineWidth);
        GUI.DrawTexture(rTop, CreatePlainColorTexture(Color.white));

        var rBottom = new Rect(rect.xMin, rect.yMax - lineWidth, rect.width, lineWidth);
        GUI.DrawTexture(rBottom, CreatePlainColorTexture(Color.white));

    }

    public static void DrawLine(Vector2 start, Vector2 end, int width, Color color)
    {
        Vector2 d = end - start;
        float a = Mathf.Rad2Deg * Mathf.Atan(d.y / d.x);
        if (d.x < 0)
            a += 180;

        int width2 = (int)Mathf.Ceil(width / 2);

        GUIUtility.RotateAroundPivot(a, start);
        GUI.DrawTexture(new Rect(start.x, start.y - width2, d.magnitude, width), CreatePlainColorTexture(color));
        GUIUtility.RotateAroundPivot(-a, start);
    }

	public static bool CanSeePoint(Vector3 from, Vector3 to, GameObject target)
	{
		RaycastHit hitInfo;
		bool b = Physics.Linecast(from, to, out hitInfo);
		if (!b || (target != null && hitInfo.collider.gameObject == target))
			return true;

		return false;
	}

	public static bool CanSeeBounds(Vector3 from, Bounds bounds, GameObject target)
	{
		RaycastHit hitInfo;
		bool b = Physics.Linecast(from, bounds.center, out hitInfo);
		if (!b || (target != null && hitInfo.collider.gameObject == target))
			return true;

		b = Physics.Linecast(from, bounds.max, out hitInfo);
		if (!b || (target != null && hitInfo.collider.gameObject == target))
			return true;

		b = Physics.Linecast(from, bounds.min, out hitInfo);
		if (!b || (target != null && hitInfo.collider.gameObject == target))
			return true;

		return false;
	}

	public static bool CanObjectSeeAnother(GameObject a, GameObject b)
	{
		RaycastHit hitInfo;
		if (Physics.Linecast(a.transform.position, b.transform.position, out hitInfo))
		{
			return hitInfo.collider.gameObject == b;
		}
		return false;
	}

	public static Rect GetCenteredRectangle(Vector2 center, float width, float height)
	{
		var rect = new Rect(center.x - width / 2, center.y - height / 2, width, height);
		return rect;
	}

	public static bool LineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
	{
		intersection = Vector2.zero;

		float Ax, Bx, Cx, Ay, By, Cy, d, e, f, num/*,offset*/;
		float x1lo, x1hi, y1lo, y1hi;
		Ax = p2.x - p1.x;
		Bx = p3.x - p4.x;
		
		// X bound box test/
		if (Ax < 0)
		{
			x1lo = p2.x;
			x1hi = p1.x;
		} 
		else
		{
			x1hi = p2.x;
			x1lo = p1.x;
		}

		if (Bx > 0)
		{
			if (x1hi < p4.x || p3.x < x1lo)
				return false;
		} 
		else
		{
			if (x1hi < p3.x || p4.x < x1lo)
				return false;
		}

		Ay = p2.y - p1.y;
		By = p3.y - p4.y;

		// Y bound box test//
		if (Ay < 0)
		{                  
			y1lo = p2.y;
			y1hi = p1.y;
			
		} 
		else
		{
			y1hi = p2.y;
			y1lo = p1.y;
		}

		if (By > 0)
		{
			if (y1hi < p4.y || p3.y < y1lo)
				return false;
		} 
		else
		{
			if (y1hi < p3.y || p4.y < y1lo)
				return false;
		}

		Cx = p1.x - p3.x;
		Cy = p1.y - p3.y;

		d = By * Cx - Bx * Cy;  // alpha numerator //
		f = Ay * Bx - Ax * By;  // both denominator //

		// alpha tests //
		if (f > 0)
		{
			if (d < 0 || d > f)
				return false;
		} 
		else
		{
			if (d > 0 || d < f)
				return false;
		}

		e = Ax * Cy - Ay * Cx;  // beta numerator //

		// beta tests //
		if (f > 0)
		{                          
			if (e < 0 || e > f)
				return false;
		} 
		else
		{
			if (e > 0 || e < f)
				return false;
		}

		// check if they are parallel
		if (f == 0)
			return false;
		
		// compute intersection coordinates //
		num = d * Ax; // numerator //
		
		//    offset = same_sign(num,f) ? f*0.5f : -f*0.5f;   // round direction //
		
		//    intersection.x = p1.x + (num+offset) / f;
		intersection.x = p1.x + num / f;

		num = d * Ay;
		
		//    offset = same_sign(num,f) ? f*0.5f : -f*0.5f;
		
		//    intersection.y = p1.y + (num+offset) / f;
		intersection.y = p1.y + num / f;

		return true;
	}

	public static bool RectLineIntersection(Rect r, Vector2 p1, Vector2 p2, out Vector2 intersection)
	{
		// top
		if(LineIntersection(r.min, new Vector2(r.xMax, r.yMin), p1, p2, out intersection))
			return true;

		// left
		if(LineIntersection(r.min, new Vector2(r.xMin, r.yMax), p1, p2, out intersection))
			return true;

		// bottom
		if(LineIntersection(r.max, new Vector2(r.xMin, r.yMax), p1, p2, out intersection))
			return true;

		// right
		if(LineIntersection(r.max, new Vector2(r.xMax, r.yMin), p1, p2, out intersection))
			return true;

		intersection = Vector2.zero;
		return false;
	}

	public static Vector2 GetAlphaMapCoords(Vector3 worldPos, Terrain terrain)
	{
		var terrainPos = terrain.transform.position;
		var terrainData = terrain.terrainData;
		
		// calculate which splat map cell the worldPos falls within (ignoring y)
		var mapX = ((worldPos.x - terrainPos.x) / terrainData.size.x * terrainData.alphamapWidth );
		var mapZ = ((worldPos.z - terrainPos.z) / terrainData.size.z * terrainData.alphamapHeight );
		
		return new Vector2(mapX, mapZ);
	}

    private static Dictionary<Color, Texture2D> plainTextureDictionary; 
    public static Texture2D CreatePlainColorTexture(Color color)
    {
        if (plainTextureDictionary == null)
            plainTextureDictionary = new Dictionary<Color, Texture2D>();

        if (plainTextureDictionary.ContainsKey(color))
            return plainTextureDictionary[color];

        var tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, color);
        tex.Apply();

        plainTextureDictionary.Add(color, tex);
        return tex;
    }

    public static Terrain GetTerrainByWorldPos(Vector3 position)
    {
        foreach (var t in Terrain.activeTerrains)
        {
            var pos = t.transform.position;
            var rect = new Rect(pos.x, pos.z, t.terrainData.heightmapWidth, t.terrainData.heightmapHeight);
            if (rect.Contains(position))
                return t;
        }
        return null;
    }

    public static float GetTerrainHeight(Vector3 position)
    {
        var t = GetTerrainByWorldPos(position);
        if (t == null)
            return 0;

        return t.SampleHeight(position) + t.transform.position.y;
    }

    void ResetTerrainColoring(Terrain t)
    {
        // reset splat maps
        var map = new float[t.terrainData.alphamapWidth, t.terrainData.alphamapHeight, 2];
        for (var y = 0; y < t.terrainData.alphamapHeight; y++)
        {
            for (var x = 0; x < t.terrainData.alphamapWidth; x++)
            {
                map[x, y, 0] = 1;
                map[x, y, 1] = 0;
            }
        }

        t.terrainData.SetAlphamaps(0, 0, map);
    }
}
