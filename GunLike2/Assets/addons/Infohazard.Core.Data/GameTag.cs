using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Infohazard.Core {
    public static class GameTag {
        public const string @Untagged = @"Untagged";
        public const string @Respawn = @"Respawn";
        public const string @Finish = @"Finish";
        public const string @EditorOnly = @"EditorOnly";
        public const string @MainCamera = @"MainCamera";
        public const string @Player = @"Player";
        public const string @GameController = @"GameController";
        public const string @Ground = @"Ground";
        public const string @item = @"item";
        public const string @bullet = @"bullet";
        public const string @HurtBox = @"HurtBox";
        public const string @EnemyWeakPoint = @"EnemyWeakPoint";
        public const string @Enemy = @"Enemy";
        public const string @voxel = @"voxel";

        public const long @UntaggedMask = 1 << 0;
        public const long @RespawnMask = 1 << 1;
        public const long @FinishMask = 1 << 2;
        public const long @EditorOnlyMask = 1 << 3;
        public const long @MainCameraMask = 1 << 4;
        public const long @PlayerMask = 1 << 5;
        public const long @GameControllerMask = 1 << 6;
        public const long @GroundMask = 1 << 7;
        public const long @itemMask = 1 << 8;
        public const long @bulletMask = 1 << 9;
        public const long @HurtBoxMask = 1 << 10;
        public const long @EnemyWeakPointMask = 1 << 11;
        public const long @EnemyMask = 1 << 12;
        public const long @voxelMask = 1 << 13;

        public static readonly string[] Tags = {
            @"Untagged", @"Respawn", @"Finish", @"EditorOnly", @"MainCamera", @"Player", @"GameController", @"Ground", @"item", @"bullet", @"HurtBox", @"EnemyWeakPoint", @"Enemy", @"voxel", 
        };

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize() {
            Tag.GameOverrideTags = Tags;
        }
    }
}
