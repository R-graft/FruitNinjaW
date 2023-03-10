using System;
using TMPro;
using UnityEngine;

namespace winterStage
{
    public class BonusController : MonoBehaviour
    {
        [Header("Ice")]
        public float iceSpeedModificator = 0.3f;
        public float iceTime = 5;

        [Header("Bomb")]
        public float maxBombDistance = 30;
        public float bombForce = 10;

        [Header("Magnete")]
        public float magneteTime = 5;

        [Header("Heart")]
        public int heartsPoolCount = 7;

        [Header("Basket")]
        public Vector2 firstDirection = new Vector2(0, 5f);
        public Vector2 secondDirection = new Vector2(5, 5f);

        [Header("Samurai")]
        public int samuraiTime = 10;

        [Header("Components")]
        [SerializeField] private BlocksController _blocks;

        [SerializeField] private SpawnSystem _spawner;

        [SerializeField] private HeartCounter _heartCounter;

        [Header("Effects")]
        [SerializeField] private GameObject _iceEffect;
        [SerializeField] private GameObject _magnetEffect;
        [SerializeField] private GameObject _heartEffect;
        [SerializeField] private Transform _bombEffect;
        [SerializeField] private TextMeshProUGUI _samuraiEffect;
        [SerializeField] private BladeHandler _blade;

        [HideInInspector] public bool _isMagnete;
        [HideInInspector] public bool _isIce;
        [HideInInspector] public bool isSamurai;

        public Action<Transform> OnBombSlash;
        public Action<Transform> OnMagnetSlash;
        public Action<Vector3> OnHeartSlash;
        public Action OnIceSlash;
        public Action<Vector3> OnBasketSlash;
        public Action OnSamuraiSlash;
        public Action OnBrickSlash;
        public Action<MimicBlock> OnMimicActive;

        private BombBonus _bomb;
        private IceBonus _ice;
        private HeartBonus _heart;
        private MagnetBonus _magnet;
        private BasketBonus _basket;
        private SamuraiBonus _samurai;
        private BrickBonus _brick;
        private MimicBonus _mimic;

        public void Init()
        {
            _ice = new IceBonus(_iceEffect, this, _blocks.AllBlocks, iceTime);
            _bomb = new BombBonus(_bombEffect, _blocks.ActiveBlocks, maxBombDistance, bombForce);
            _heart = new HeartBonus(_heartEffect, _heartCounter, transform, heartsPoolCount);
            _magnet = new MagnetBonus(_magnetEffect, _blocks, this, magneteTime);
            _basket = new BasketBonus(_blocks, _spawner, firstDirection, secondDirection);
            _samurai = new SamuraiBonus(_samuraiEffect, _spawner, _blocks, this, samuraiTime);
            _brick = new BrickBonus(_blade);
        }

        private void IceBonus()
        {
            if (!_isIce)
            {
                _isIce = true;

                StartCoroutine(_ice.IceBonusAction());
            }
            else
            {
                _ice.iceTime = iceTime;
            }
        }
        private void BombBonus(Transform bombTransform)
        {
            _bomb.BombBonusAction(bombTransform);
        }

        private void HeartBonus(Vector3 heartPos)
        {
            _heart.HeartBonusAction(heartPos);
        }

        private void MagnetBonus(Transform magnetPos)
        {
            if (!_isMagnete)
            {
                _isMagnete = true;

                StartCoroutine(_magnet.MagneteBonusAction(magnetPos));
            }
            else
            {
                _magnet.magneteTime = magneteTime;
            }
        }

        private void BasketBonus(Vector3 basketPos)
        {
            _basket.BasketBonusAction(basketPos);
        }

        private void SamuraiBonus()
        {
            if (!isSamurai)
            {
                isSamurai = true;

                StartCoroutine(_samurai.SamuraiBonusAction());
            }
        }

        private void MimicBonus(MimicBlock block)
        {
            _mimic = new MimicBonus(_spawner, _blocks);

            StartCoroutine(_mimic.MimicAction(block));

            StartCoroutine(_mimic.MimicMove(block));
        }

        private void BrickBonus()
        {
            StartCoroutine(_brick.BrickAction());
        }

        private void OnEnable()
        {
            OnBombSlash += BombBonus;
            OnIceSlash += IceBonus;
            OnMagnetSlash += MagnetBonus;
            OnHeartSlash += HeartBonus;
            OnBasketSlash += BasketBonus;
            OnSamuraiSlash += SamuraiBonus;
            OnBrickSlash += BrickBonus;
            OnMimicActive += MimicBonus;
        }

        private void OnDisable()
        {
            OnBombSlash -= BombBonus;
            OnIceSlash -= IceBonus;
            OnMagnetSlash -= MagnetBonus;
            OnHeartSlash -= HeartBonus;
            OnBasketSlash -= BasketBonus;
            OnSamuraiSlash -= SamuraiBonus;
            OnBrickSlash -= BrickBonus;
            OnMimicActive -= MimicBonus;
        }
    }
}
