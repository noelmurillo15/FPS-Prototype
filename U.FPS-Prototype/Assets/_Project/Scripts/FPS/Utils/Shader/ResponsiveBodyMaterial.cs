/*
 * ResponsiveBodyMaterial -
 * Created by : Allan N. Murillo
 * Last Edited : 3/26/2020
 */

using UnityEngine;
using ANM.FPS.Npc;
using ANM.FPS.Player;

namespace ANM.Utils.Shader
{
    public class ResponsiveBodyMaterial : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _renderer;
        [SerializeField] private bool isPlayer = false;
        [SerializeField] private Color defaultTintColor;
        [SerializeField] private Color damagedTintColor;
        [SerializeField] private Color healedTintColor;
        [SerializeField] private float intensity = 2f;
        private Color _currentTintColor;
        private Color _targetTintColor;
        private static readonly int Tint = UnityEngine.Shader.PropertyToID("_EmissionColor");


        private void OnEnable()
        {
            if (isPlayer)
            {
                var player = GetComponentInParent<PlayerMaster>();
                player.PlayerHpDeductionEvent += OnPlayerHpDeduction;
                player.PlayerHpIncreaseEvent += OnPlayerHpIncrease;
            }
            else
            {
                var npc = GetComponentInParent<NpcMaster>();
                npc.EventNpcTakeDmg += OnEnemyHpDeduction;
                npc.EventNpcHealthRecovered += OnEnemyHpIncrease;
            }

            Reset();
        }

        private void OnDisable()
        {
            //    TODO : de-registering is happening when the Master Scripts have already been destroyed
            if (isPlayer)
            {
                var player = GetComponentInParent<PlayerMaster>();
                if (player == null) return;
                player.PlayerHpDeductionEvent -= OnPlayerHpDeduction;
                player.PlayerHpIncreaseEvent -= OnPlayerHpIncrease;
            }
            else
            {
                var npc = GetComponentInParent<NpcMaster>();
                if (npc == null) return;
                npc.EventNpcTakeDmg -= OnEnemyHpDeduction;
                npc.EventNpcHealthRecovered -= OnEnemyHpIncrease;
            }
        }

        private void OnPlayerHpDeduction(float hp)
        {
            SetBodyTint(damagedTintColor * intensity);
            if (IsInvoking(nameof(Reset))) CancelInvoke(nameof(Reset));
            Invoke(nameof(Reset), 0.05f);
        }

        private void OnPlayerHpIncrease(float hp)
        {
            SetBodyTint(healedTintColor * intensity);
            if (IsInvoking(nameof(Reset))) CancelInvoke(nameof(Reset));
            Invoke(nameof(Reset), 0.05f);
        }

        private void OnEnemyHpDeduction(int hp)
        {
            SetBodyTint(damagedTintColor * intensity);
            if (IsInvoking(nameof(Reset))) CancelInvoke(nameof(Reset));
            Invoke(nameof(Reset), 0.05f);
        }

        private void OnEnemyHpIncrease()
        {
            SetBodyTint(healedTintColor * intensity);
            if (IsInvoking(nameof(Reset))) CancelInvoke(nameof(Reset));
            Invoke(nameof(Reset), 0.05f);
        }

        private void SetBodyTint(Color color)
        {
            _targetTintColor = color;
            _renderer.material.SetColor(Tint, _targetTintColor);
        }

        private void Reset()
        {
            SetBodyTint(defaultTintColor);
        }
    }
}
