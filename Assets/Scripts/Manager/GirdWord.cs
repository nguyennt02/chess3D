using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class GirdWord : MonoBehaviour
{
    [SerializeField] int2 girdSoup;
    [SerializeField] float2 scale;
    [SerializeField] float3 offset;
    [SerializeField] float3 axis;
    [SerializeField] float angle;
    NativeArray<int> _gridValue; // [0, 0, 0, 1, 0, 0, 0]

    float3x3 originalMatrix;
    float3x3 rotationMatrix;

    public int EmmtyValue = -1;

    public void InitGird(int2 girdSoup, float2 scale)
    {
        this.girdSoup = girdSoup;
        this.scale = scale;

        _gridValue = new NativeArray<int>(girdSoup.x * girdSoup.y, Allocator.Persistent);

        originalMatrix = GetRotationMatrix(axis, -angle);
        rotationMatrix = GetRotationMatrix(axis, angle);

        offset = new float3(
            -scale.x * (girdSoup.x - 1) / 2 - transform.position.x,
            -scale.y * (girdSoup.y - 1) / 2 - transform.position.y,
            0);
    }

    private void OnDestroy()
    {
        _gridValue.Dispose();
    }

    public int2 ConvertIndexToGridPos(in int index)
    {
        int x = index % girdSoup.x;
        int y = index / girdSoup.x;
        return new int2(x, y);
    }

    public float3 ConvertGirdPosToWordPos(in int2 girdPos)
    {
        float3 originalPosition = new float3(
            girdPos.x * scale.x + offset.x,
            girdPos.y * scale.y + offset.y,
            0);

        float3 relativePosition = originalPosition - (float3)transform.position;
        float3 rotatedPosition = math.mul(rotationMatrix, relativePosition);
        float3 finalPosition = rotatedPosition + (float3)transform.position;
        return finalPosition;
    }

    public float3 ConvertIndexToWordPos(in int index)
    {
        int2 girdPos = ConvertIndexToGridPos(index);
        float3 position = ConvertGirdPosToWordPos(girdPos);
        return position;
    }

    public int2 ConvertWordPosToGirdPos(in float3 wordPos)
    {
        float3 rotatedPosition = wordPos - (float3)transform.position;
        float3 relativePosition = math.mul(originalMatrix, rotatedPosition);
        float3 originalPosition = relativePosition + (float3)transform.position;
        float3 adjustedPosition = originalPosition - offset;

        int x = Mathf.RoundToInt(adjustedPosition.x / scale.x);
        int y = Mathf.RoundToInt(adjustedPosition.y / scale.y);

        return new int2(x, y);
    }

    public int ConvertGirdPosToIndex(in int2 girdPos)
    {
        int index = girdPos.x + girdPos.y * girdSoup.x;
        return index;
    }

    public int ConverWordPosToIndex(in float3 wordPos)
    {
        int2 girdPos = ConvertWordPosToGirdPos(wordPos);
        int index = ConvertGirdPosToIndex(girdPos);
        return index;
    }

    public int GetValue(int index)
    {
        return _gridValue[index];
    }

    public int GetValue(int2 gridPos)
    {
        int index = ConvertGirdPosToIndex(gridPos);
        return _gridValue[index];
    }

    public int GetValue(float3 wordPos)
    {
        int index = ConverWordPosToIndex(wordPos);
        return _gridValue[index];
    }

    public void SetValue(int index, int value)
    {
        _gridValue[index] = value;
    }

    public void SetValue(int2 girdPos, int value)
    {
        int index = ConvertGirdPosToIndex(girdPos);
        SetValue(index, value);
    }

    public void SetValue(float3 wordPos, int value)
    {
        int index = ConverWordPosToIndex(wordPos);
        SetValue(index, value);
    }

    public bool IsGirdPosOutsideAt(int2 girdPos)
    {
        if (girdPos.x < 0 || girdPos.x >= girdSoup.x
        || girdPos.y < 0 || girdPos.y >= girdSoup.y) return true;
        return false;
    }

    public bool IsWordPosOutsideAt(float3 wordPos)
    {
        int2 girdPos = ConvertWordPosToGirdPos(wordPos);
        return IsGirdPosOutsideAt(girdPos);
    }

    public bool IsIndexOutsideAt(int index)
    {
        int2 girdPos = ConvertIndexToGridPos(index);
        return IsGirdPosOutsideAt(girdPos);
    }

    public bool IsIndexEmpty(int index)
    {
        if (IsIndexOutsideAt(index)) return true;
        var value = GetValue(index);
        return value == EmmtyValue;
    }

    public bool IsGirdPosEmpty(int2 girdPos)
    {
        if (IsGirdPosOutsideAt(girdPos)) return false;
        var value = GetValue(girdPos);
        return value == EmmtyValue;
    }

    public bool IsWordPosEmpty(float3 wordPos)
    {
        if (IsWordPosOutsideAt(wordPos)) return true;
        var value = GetValue(wordPos);
        return value == EmmtyValue;
    }

    public float3x3 GetRotationMatrix(float3 axis, float angle)
    {
        // Chuẩn hóa trục quay
        float3 u = math.normalize(axis);
        float radianAngle = math.radians(angle);

        // Tính các giá trị cos và sin
        float cosTheta = math.cos(radianAngle);
        float sinTheta = math.sin(radianAngle);

        // Các thành phần của trục
        float ux = u.x;
        float uy = u.y;
        float uz = u.z;

        float3 c1 = new float3(
                cosTheta + ux * ux * (1 - cosTheta),
                ux * uy * (1 - cosTheta) - uz * sinTheta,
                ux * uz * (1 - cosTheta) + uy * sinTheta
            );

        float3 c2 = new float3(
                uy * ux * (1 - cosTheta) + uz * sinTheta,
                cosTheta + uy * uy * (1 - cosTheta),
                uy * uz * (1 - cosTheta) - ux * sinTheta
            );

        float3 c3 = new float3(
                uz * ux * (1 - cosTheta) - uy * sinTheta,
                uz * uy * (1 - cosTheta) + ux * sinTheta,
                cosTheta + uz * uz * (1 - cosTheta)
            );

        // Tạo ma trận xoay
        return new float3x3(c1, c2, c3);
    }

    public int2 NeighborAt(int2 current, int2 dir)
    {
        int2 neighbor = current + dir;
        return neighbor;
    }

    public int2[] LineNeighbor(int2 current, int2 dir)
    {
        List<int2> neighbors = new();
        int2 neighbor = NeighborAt(current, dir);
        while (!IsGirdPosOutsideAt(neighbor))
        {
            neighbors.Add(neighbor);
            if (!IsGirdPosEmpty(neighbor)) break;
            neighbor = NeighborAt(neighbor, dir);
        }
        return neighbors.ToArray();
    }
}
