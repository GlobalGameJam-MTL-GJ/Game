/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID LEVEL_DOOR = 4002963088U;
        static const AkUniqueID LEVEL_SPAWN = 1976190133U;
        static const AkUniqueID MUSIC_GAMEOVER = 2897915005U;
        static const AkUniqueID MUSIC_LEVEL = 2177735725U;
        static const AkUniqueID NPC_BELL = 3226713398U;
        static const AkUniqueID PLAYER_DROP = 1922690190U;
        static const AkUniqueID PLAYER_FOOTSTEPS = 1730208058U;
        static const AkUniqueID PLAYER_PICKUP = 1627434233U;
        static const AkUniqueID PLAYER_THROW = 65181031U;
        static const AkUniqueID PROP_IMPACT = 986777071U;
        static const AkUniqueID UI_HOVER = 2118900976U;
        static const AkUniqueID UI_SELECT = 2774129122U;
        static const AkUniqueID UI_STARTGAME = 2467171692U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace GAMEOVER
        {
            static const AkUniqueID GROUP = 4158285989U;

            namespace STATE
            {
                static const AkUniqueID FALSE = 2452206122U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID TRUE = 3053630529U;
            } // namespace STATE
        } // namespace GAMEOVER

        namespace ROOMTYPE
        {
            static const AkUniqueID GROUP = 1992536912U;

            namespace STATE
            {
                static const AkUniqueID BACKSTORE = 2915384035U;
                static const AkUniqueID FRONTSTORE = 3572512317U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace ROOMTYPE

    } // namespace STATES

    namespace SWITCHES
    {
        namespace GAMESTATE
        {
            static const AkUniqueID GROUP = 4091656514U;

            namespace SWITCH
            {
                static const AkUniqueID GAMEEND = 2197986718U;
                static const AkUniqueID GAMEPLAY = 89505537U;
            } // namespace SWITCH
        } // namespace GAMESTATE

        namespace PROPTYPE
        {
            static const AkUniqueID GROUP = 3381216982U;

            namespace SWITCH
            {
                static const AkUniqueID BOW = 546945295U;
                static const AkUniqueID CROWN = 2848349022U;
                static const AkUniqueID GRIMOIRE = 943678455U;
                static const AkUniqueID POTION = 4272075576U;
                static const AkUniqueID SWORD = 2454616260U;
            } // namespace SWITCH
        } // namespace PROPTYPE

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID CLOCK = 1182830809U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID LEVEL = 2782712965U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID NPC = 662417162U;
        static const AkUniqueID PLAYER = 1069431850U;
        static const AkUniqueID PROPS = 968010305U;
        static const AkUniqueID UI = 1551306167U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID DEFAULT_MOTION_DEVICE = 4230635974U;
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
