using System;
using UnityEngine;

public static class CityProcGenUtils
{
    public static BuildingWallSegment CreateWallSegment(GameObject wallPrefab, Transform parent, Vector3 pos, Quaternion rot, float length, float height, float thickness = 1.0f)
    {
        var wallSegment = new BuildingWallSegment();

        var scale = new Vector3(length, height, thickness);

        var wallSegmentGameObj = GameObjectUtils.InstantiateTRS(wallPrefab, parent, pos, rot, scale);
        wallSegment.GameObjectInstance = wallSegmentGameObj;

        return wallSegment;
    }

    public static BuildingWall CreateWallWithDoor(GameObject wallPrefab, BuildingDoorDefinition doorDefinition, Transform transform, Edge2 edge, float originY, float doorT, float height, float thickness = 1.0f)
    {
        var edgeRot = Quaternion.Euler(0.0f, edge.Angle, 0.0f);
        var doorPos = edge.Lerp(doorT).ToVec3XZ(originY);

        var wall = new BuildingWall();

        {
            var door = new BuildingDoor();
            wall.Doors.Add(door);

            var doorGameObj = GameObject.Instantiate(doorDefinition.Prefab, doorPos, edgeRot, transform);
            door.GameObjectInstance = doorGameObj;
        }

        {
            var doorWallSegmentPos = doorPos + Vector3.up * doorDefinition.Height;
            var doorWallSegmentHeight = (height - doorDefinition.Height);

            var doorWallSegment = CityProcGenUtils.CreateWallSegment(wallPrefab, transform, doorWallSegmentPos, edgeRot, doorDefinition.Width, doorWallSegmentHeight, thickness);
            wall.WallSegments.Add(doorWallSegment);
        }

        {
            var wallLengthA = Mathf.Lerp(0.0f, edge.Length, doorT) - doorDefinition.Width * 0.5f;
            var wallSegmentPosA = edge.Lerp((wallLengthA * 0.5f) / edge.Length).ToVec3XZ(originY);

            var wallSegmentA = CityProcGenUtils.CreateWallSegment(wallPrefab, transform, wallSegmentPosA, edgeRot, wallLengthA, height, thickness);
            wall.WallSegments.Add(wallSegmentA);

            var wallLengthB = Mathf.Lerp(0.0f, edge.Length, 1.0f - doorT) - doorDefinition.Width * 0.5f;
            var wallSegmentPosB = edge.Lerp(1.0f - ((wallLengthB * 0.5f) / edge.Length)).ToVec3XZ(originY);

            var wallSegmentB = CityProcGenUtils.CreateWallSegment(wallPrefab, transform, wallSegmentPosB, edgeRot, wallLengthB, height, thickness);
            wall.WallSegments.Add(wallSegmentB);
        }

        return wall;
    }
}
