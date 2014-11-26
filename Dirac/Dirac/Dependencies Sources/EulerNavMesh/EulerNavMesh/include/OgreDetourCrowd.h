/*
    OgreCrowd
    ---------

    Copyright (c) 2012 Jonas Hauquier

    Additional contributions by:

    - mkultra333
    - Paul Wilson

    Sincere thanks and to:

    - Mikko Mononen (developer of Recast navigation libraries)

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.

*/

#ifndef OGREDETOURCROWD_H
#define OGREDETOURCROWD_H

#include "OgreRecastDefinitions.h"
#include "OgreRecast.h"
#include "DetourCrowd/DetourCrowd.h"
#include <vector>


/**
  * Ogre wrapper around DetourCrowd.
  * Controls a crowd of agents that can steer to avoid each other and follow
  * individual paths.
  *
  * This class is largely based on the CrowdTool used in the original recastnavigation
  * demo.
  **/
class OgreDetourCrowd
{
public:
    /**
      * Initialize a detour crowd that will manage agents on the specified
      * recast navmesh. It does not matter how this navmesh is constructed
      * (either with OgreRecast directly or with DetourTileCache).
      * Parameters such as agent dimensions will be taken from the specified
      * recast component.
      **/
    OgreDetourCrowd(OgreRecast *recast);
    ~OgreDetourCrowd(void);

    /**
      * Add an agent to the crowd
      * Returns ID of created agent (-1 if maximum agents is already created)
      **/
    int addAgent(const Vector3 position);

    /**
      * Retrieve agent with specified ID from the crowd.
      **/
    const dtCrowdAgent* getAgent(int id);

    /**
      * Remove agent with specified ID from the crowd.
      **/
    void removeAgent(const int idx);

    /**
      * Reference to the DetourCrowd object that is wrapped.
      **/
    dtCrowd* m_crowd;

    /**
      * Reference to the Recast/Detour wrapper object for Ogre.
      **/
    OgreRecast *m_recast;

    /**
      * The latest set target or destination section in the recast navmesh.
      **/
    dtPolyRef m_targetRef;

    /**
      * The latest set target or destination position.
      **/
    float m_targetPos[3];

    /**
      * Max pathlength for calculated paths.
      **/
    static const int AGENT_MAX_TRAIL = 64;

    /**
      * Max number of agents allowed in this crowd.
      **/
    static const int MAX_AGENTS = 128;

    /**
      * Stores the calculated paths for each agent in the crowd.
      **/
    struct AgentTrail
    {
            float trail[AGENT_MAX_TRAIL*3];
            int htrail;
    };
    AgentTrail m_trails[MAX_AGENTS];

    /**
      * Debug info object used in the original recast/detour demo, not used in this
      * application.
      **/
    dtCrowdAgentDebugInfo m_agentDebug;

    /**
      * Parameters for obstacle avoidance of DetourCrowd steering.
      **/
    dtObstacleAvoidanceDebugData* m_vod;


    // Agent configuration parameters
    bool m_anticipateTurns;
    bool m_optimizeVis;
    bool m_optimizeTopo;
    bool m_obstacleAvoidance;
    bool m_separation;

    float m_obstacleAvoidanceType;
    float m_separationWeight;


protected:
    /**
      * Helper to calculate the needed velocity to steer an agent to a target destination.
      * Parameters:
      *     velocity    is the return parameter, the calculated velocity
      *     position    is the current position of the agent
      *     target      is the target destination to reach
      *     speed       is the (max) speed the agent can travel at
      *
      * This function can be used together with requestMoveVelocity to achieve the functionality
      * of the old adjustMoveTarget function.
      **/
    static void calcVel(float* velocity, const float* position, const float* target, const float speed);


private:
    /**
      * Number of (active) agents in the crowd.
      **/
    int m_activeAgents;
};

#endif // OGREDETOURCROWD_H
