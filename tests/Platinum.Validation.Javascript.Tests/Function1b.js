﻿function ( value ) {
    if ( value === null ) {
        return true;
    }

    if ( value.length == 0 ) {
        return true;
    }

    return value[ 0 ] == '1';
}
