#pragma once

    struct Vector3
    {
    public:
		float x, y, z;

    public:
        inline Vector3()
        {
        };

        inline Vector3( const float fX, const float fY, const float fZ )
            : x( fX ), y( fY ), z( fZ )
        {
        };
	};

