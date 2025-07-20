using System;
using System.Collections.Generic;
using System.Linq;
using Client;
using Common;
using Logic;
using Logic.ActionRequests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.DebugWindows
{
    public class DebugActionMove : MonoBehaviour
    {
        [SerializeField] private Button _buttonExecute;
        [SerializeField] private TextMeshProUGUI _textIds;
        [SerializeField] private TMP_InputField _input;
        [SerializeField] private TMP_InputField _inputX;
        [SerializeField] private TMP_InputField _inputY;

        private IActionRequestSender _actionRequestSender;
        private BattleInstanceClient _battleInstance;
        private void Awake()
        {
            _battleInstance = AutoResolver.Resolve<BattleInstanceClient>();
            _actionRequestSender = AutoResolver.Resolve<IActionRequestSender>();
            _buttonExecute.onClick.AddListener(HandleButtonExecuteClicked);
        }

        private void OnDestroy()
        {
            _buttonExecute.onClick.RemoveListener(HandleButtonExecuteClicked);
        }

        private void Update()
        {
            if (_battleInstance == null)
            {
                return;
            }
            _textIds.text = _battleInstance.ModelClient.UnitEntityModels.Aggregate("", (current, model) => current + model.Key);
        }

        private void HandleButtonExecuteClicked()
        {
            var path = new List<Vector2Int> { new (Convert.ToInt32(_inputX.text), Convert.ToInt32(_inputY.text) ) };
            _actionRequestSender.SendActionRequest(new ActionRequestMove
            {
                CasterId = Convert.ToInt32(_input.text),
                Path = path
            });
        }
    }
}