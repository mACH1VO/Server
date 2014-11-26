
#ifndef CROWDMANAGER_H
#define CROWDMANAGER_H

class OgreRecast;
class OgreDetourCrowd;
class OgreDetourTileCache;

// TODO maybe add support for "persistent" characters, eg. those that are always visible, or at least of which the agent and AI remains when out of view distance

/**
  * Manages and instances a crowd of characters in the vicinity of the camera.
  *
  **/
class CrowdManager
{
public:
    CrowdManager(OgreDetourTileCache *tileCache);

    void update(float timeSinceLastFrame);

    int getNbLoadedTiles(void);

    int getNbBorderTiles(void);

    void setDebugVisibility(bool visible);

    /**
      * The size of the crowd
      **/
    int getSize(void) { return mCrowdSize; }

    int getNbAssignedAgents(void) { return mAssignedCharacters.size(); }

    int getGridDimensions(void) { return mDimension; }



    // TODO define setters for these?
    static const float CROWD_PAGE_UPDATE_DELTA;
    static const float MAX_CROWD_SIZE;
    static const float RADIUS_EPSILON;

    static bool HUMAN_CHARACTERS;
    static bool INSTANCED_CROWD;

    // TODO move this struct to OgreDetourTileCache??
    // TODO functionality corresponds largely to OgreDetourTileCache::TileSelection, merge them without breaking anything
    struct NavmeshTileSet
    {
        int xMin;   // Min tile X index (inclusive)
        int yMin;   // Min tile Y index (inclusive)
        int xMax;   // Max tile X index (inclusive)
        int yMax;   // Min tile Y index (inclusive)

        int getXWidth(void) { return 1+ xMax - xMin ; }
        int getYWidth(void) { return 1+ yMax - yMin; }
        int getNbTiles(void) { return getXWidth()*getYWidth(); }
    };

protected:
    /**
      * Calculate the navmesh tile-aligned bounding area around the
      * current camera position that has to be populated with crowd agents.
      **/
    NavmeshTileSet calculatePopulatedArea(void);

    bool updatePagedCrowd(float timeSinceLastFrame);

    void loadAgents(int tx, int ty, int nbAgents);

    void unloadAgents(int tx, int ty);

    Ogre::AxisAlignedBox getNavmeshTileSetBounds(NavmeshTileSet tileSet);

    /**
      * Determines whether the tile with specified grid coordinates exists
      * and is loaded (in the tilecache).
     **/
    virtual bool tileExists(int tx, int ty);

    void updatePagedAreaDebug(NavmeshTileSet pagedArea);

    void debugPrint(Ogre::String message);

    Ogre::String tileToStr(int tx, int ty);

    void unloadAgentsOutsideArea(NavmeshTileSet area);

    NavmeshTileSet getExistingArea(NavmeshTileSet area);

    void updateBorderTiles(void);

    virtual bool walkedOffGrid(const Character* character);

    virtual void initAgents(void);


    OgreDetourTileCache *mDetourTileCache;
    OgreRecast *mRecast;
    OgreDetourCrowd *mDetourCrowd;


    NavmeshTileSet mCurrentlyPagedArea;

    /**
      * Size (in number of navmesh tiles) in x and y direction
      * that the paged area centered around camera position will
      * continue and will be populated with agents.
      * Defines the size of the area to be populated with agents.
      **/
    int mPagedAreaDistance;
        // TODO allow other methods of selecting area to page? Like a circle around camera position

    int mNbPagedTiles;

    int mNbTilesInBorder;

    int mCrowdSize;

    int mDimension;

};

#endif // CROWDMANAGER_H
