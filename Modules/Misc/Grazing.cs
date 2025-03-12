﻿using System;
using System.Collections.Generic;
using System.Text;
using Grate.Extensions;
using Grate.GUI;
using Grate.Networking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Video;
using static Grate.Modules.Misc.CatMeow;

namespace Grate.Modules.Misc
{
    class Grazing : GrateModule
    {
        GrazeHandler LocalGraze;
        void Awake()
        {
            NetworkPropertyHandler.Instance.OnPlayerModStatusChanged += OnPlayerModStatusChanged;
            Patches.VRRigCachePatches.OnRigCached += OnRigCached;
        }

        public override string GetDisplayName()
        {
            return "Gwazywazy";
        }
        protected override void OnEnable()
        {
            if (!MenuController.Instance.Built) return;
            base.OnEnable();
            LocalGraze = GorillaTagger.Instance.offlineVRRig.AddComponent<GrazeHandler>();
        }
        public override string Tutorial()
        {
            return "I am me maker of this yes";
        }
        private void OnRigCached(NetPlayer player, VRRig rig)
        {
            if (rig?.gameObject?.GetComponent<GrazeHandler>() != null)
            {
                rig?.gameObject?.GetComponent<GrazeHandler>()?.vp.Obliterate();
                rig?.gameObject?.GetComponent<GrazeHandler>()?.Obliterate();
            }
        }
        private void OnPlayerModStatusChanged(NetPlayer player, string mod, bool enabled)
        {
            if (mod == GetDisplayName() && player.UserId == "42D7D32651E93866")
            {
                if (enabled)
                {
                    player.Rig().gameObject.GetOrAddComponent<GrazeHandler>();
                }
                else
                {
                    player.Rig().gameObject.GetComponent<GrazeHandler>().vp.gameObject.Obliterate();
                    player.Rig().gameObject.GetComponent<GrazeHandler>().Obliterate();
                }
            }
        }
        protected override void Cleanup()
        {
            LocalGraze?.vp.Obliterate();
            LocalGraze?.Obliterate();
        }

        class GrazeHandler : MonoBehaviour
        {
            public VideoPlayer vp;
            NetworkedPlayer np;
            void Start()
            {
                vp = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<VideoPlayer>();
                vp.GetComponent<Collider>().Obliterate();
                vp.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                vp.source = VideoSource.Url;
                vp.targetMaterialRenderer = vp.GetComponent<Renderer>();
                vp.url = "https://graze.gay/vid.mp4";
                vp.loopPointReached += delegate { vp.Play(); };
                vp.Play();
                vp.transform.SetParent(transform);
                vp.transform.localPosition = Vector3.zero;
                vp.transform.localRotation = Quaternion.Euler(Vector3.zero);    
            }
            void Update()
            {
                if (np == null)
                {
                    np = gameObject.GetComponent<NetworkedPlayer>();
                }
                else
                {
                    if (np.owner.UserId != "42D7D32651E93866")
                    {
                        vp.gameObject.Obliterate();
                        this.Obliterate();
                    }
                }
            }
            void OnDestroy()
            {
                vp.gameObject.Obliterate();
            }
            void OnDisable()
            {
                vp.gameObject.Obliterate();
            }
        }
    }
}
