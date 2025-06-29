using System;
using Exiled.API.Features;
using ProjectMER.Features;
using ProjectMER.Features.Objects;
using UnityEngine;

namespace Scp066.Features.Manager;
public static class SchematicManager
{
    public static SchematicObject AddSchematicByName(string schematicName)
    {
        try
        {
            return ObjectSpawner.SpawnSchematic(schematicName, Vector3.zero, Vector3.zero, Vector3.one);
        }
        catch (Exception ex)
        {
            Log.Error($"An error occurred when loading schematics: {ex}");
            return null;
        }
    }
}