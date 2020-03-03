using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;
using System;


public class ObstacleTowerSettings : MonoBehaviour
{

    public FloorBuilder floor;

    public const int MaxSeed = 99999;
    public const int MaxFloors = 100;

    private ObstacleTowerAgent agentComponent;

    //This has been added as part of migration by me (Cristiana)
    IFloatProperties m_ResetParameters;

    private void Awake()
    {

        m_ResetParameters = Academy.Instance.FloatProperties;

        //from Initialize(Academy)
        floor.environmentParameters = new EnvironmentParameters();

        agentComponent = FindObjectOfType<ObstacleTowerAgent>();
        GraphicsSettings.useScriptableRenderPipelineBatching = true;
        
        //added from previous virtual method reset
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
    }


    public void EnvironmentReset()
    {
        Debug.Log("Academy resetting");
        agentComponent.denseReward = Mathf.Clamp((int)m_ResetParameters.GetPropertyWithDefault("dense-reward", 1), 0, 1) != 0;
        //you can't check at the moment whether you're doing inference or not...
        //if (GetIsInference())
        //{
        //    EnableInference();
        //}
        //else
        //{
        //    EnableTraining();
        //}

        var towerSeed = Mathf.Clamp((int)m_ResetParameters.GetPropertyWithDefault("tower-seed", -1), -1, MaxSeed);
        var totalFloors = Mathf.Clamp((int)m_ResetParameters.GetPropertyWithDefault("total-floors", 100), 1, MaxFloors);
        var startingFloor = Mathf.Clamp((int)m_ResetParameters.GetPropertyWithDefault("starting-floor", 0), 0, totalFloors);

        UpdateEnvironmentParameters();

        if (totalFloors > 0 && totalFloors < MaxFloors)
        {
            floor.totalFloors = totalFloors;
        }
        if (startingFloor < floor.totalFloors && startingFloor >= 0)
        {
            floor.startingFloorNumber = startingFloor;
        }

        bool validSeed = towerSeed < MaxSeed;
        if (towerSeed != -1 && validSeed)
        {
            floor.fixedTower = true;
            floor.towerNumber = towerSeed;
        }
        else
        {
            floor.fixedTower = false;
        }
    }

    private void SetDefaultEnvironmentParameters()
    {
        floor.environmentParameters.lightingType = LightingType.Dynamic;
        floor.environmentParameters.themeParameter = VisualThemeParameter.Serial;
        floor.environmentParameters.agentPerspective = AgentPerspective.ThirdPerson;
        floor.environmentParameters.allowedRoomTypes = AllowedRoomTypes.PlusPuzzle;
        floor.environmentParameters.allowedRoomModules = AllowedRoomModules.All;
        floor.environmentParameters.allowedFloorLayouts = AllowedFloorLayouts.PlusCircling;
        floor.environmentParameters.defaultTheme = VisualTheme.Ancient;
    }

    private void EnableInference() //not called anymore
    {
        //SetIsInference(true);
        agentComponent.SetInference();
        //Time.captureFramerate = 0;
    }

    private void EnableTraining() //not called anymore
    {
        //SetIsInference(false);
        agentComponent.SetTraining();
        //Time.captureFramerate = 60;
    }



    private void UpdateEnvironmentParameters()
    {
        if (Enum.IsDefined(typeof(LightingType), (int)m_ResetParameters.GetPropertyWithDefault("lighting-type", 1)))
        {
            floor.environmentParameters.lightingType = (LightingType)m_ResetParameters.GetPropertyWithDefault("lighting-type", 1);
        }
        else
        {
            Debug.Log("lighting-type outside of valid range. Using default value.");
        }

        if (Enum.IsDefined(typeof(VisualThemeParameter), (int)m_ResetParameters.GetPropertyWithDefault("visual-theme", 1)))
        {
            floor.environmentParameters.themeParameter = (VisualThemeParameter)m_ResetParameters.GetPropertyWithDefault("visual-theme", 1);
        }
        else
        {
            Debug.Log("visual-theme outside of valid range. Using default value.");
        }

        if (Enum.IsDefined(typeof(AgentPerspective), (int)m_ResetParameters.GetPropertyWithDefault("agent-perspective", 1)))
        {
            floor.environmentParameters.agentPerspective = (AgentPerspective)m_ResetParameters.GetPropertyWithDefault("agent-perspective", 1);
        }
        else
        {
            Debug.Log("agent-perspective outside of valid range. Using default value.");
        }

        if (Enum.IsDefined(typeof(AllowedRoomTypes), (int)m_ResetParameters.GetPropertyWithDefault("allowed-rooms", 2)))
        {
            floor.environmentParameters.allowedRoomTypes = (AllowedRoomTypes)m_ResetParameters.GetPropertyWithDefault("allowed-rooms", 2);
        }
        else
        {
            Debug.Log("allowed-rooms outside of valid range. Using default value.");
        }

        if (Enum.IsDefined(typeof(AllowedRoomModules), (int)m_ResetParameters.GetPropertyWithDefault("allowed-modules", 2)))
        {
            floor.environmentParameters.allowedRoomModules = (AllowedRoomModules)m_ResetParameters.GetPropertyWithDefault("allowed-modules", 2);
        }
        else
        {
            Debug.Log("allowed-modules outside of valid range. Using default value.");
        }

        if (Enum.IsDefined(typeof(AllowedFloorLayouts), (int)m_ResetParameters.GetPropertyWithDefault("allowed-modules", 2)))
        {
            floor.environmentParameters.allowedFloorLayouts = (AllowedFloorLayouts)m_ResetParameters.GetPropertyWithDefault("allowed-floors", 2);
        }
        else
        {
            Debug.Log("allowed-floors outside of valid range. Using default value.");
        }

        if (Enum.IsDefined(typeof(VisualTheme), (int)m_ResetParameters.GetPropertyWithDefault("default-theme", 0)))
        {
            floor.environmentParameters.defaultTheme = (VisualTheme)m_ResetParameters.GetPropertyWithDefault("default-theme", 0);
        }
        else
        {
            Debug.Log("default-theme outside of valid range. Using default value.");
        }
    }

}
