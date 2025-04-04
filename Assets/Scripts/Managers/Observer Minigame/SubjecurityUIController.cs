using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ObserverMinigame
{
    public class SubjecurityUIController : MonoBehaviour, ISubject
    {
        [SerializeField] Button notifyButton;
        [SerializeField] Button quitTerminalButton;
        [SerializeField] TextMeshProUGUI doneSubscriptionsText;
        [SerializeField] TextMeshProUGUI numberOfConsolesText;

        public Action<InputAction.CallbackContext> OnQuitTerminal;

        //Observer Patten related
        List<IObserver> observers = new List<IObserver>();

        int subscribedConsoles = 0;
        public int GetSubscribedConsoles => subscribedConsoles;
        int numberOfConsoles = 0;

        public void SetUp(int numberOfConsoles)
        {
            this.numberOfConsoles = numberOfConsoles;
            numberOfConsolesText.text = GetStringNumber(numberOfConsoles);
        }

        private void OnEnable()
        {
            CursorVisibility.ShowCursor();
        }

        private void OnDisable()
        {
            if(subscribedConsoles != numberOfConsoles) CursorVisibility.HideCursor();
        }

        public void CloseTerminal()
        {
            OnQuitTerminal.Invoke(new InputAction.CallbackContext());
        }

        //Win condition
        public void NotifyClick()
        {
            NotifyObservers();
            GameObject.FindAnyObjectByType<GameManager>().GameOver(false);
        }

        void Update()
        {
            if (!Cursor.visible) CursorVisibility.ShowCursor();
        }

        void UpdateSubscriptions()
        {
            doneSubscriptionsText.text = GetStringNumber(observers.Count);
        }

        public void AddObserver(IObserver observer)
        {
            if(!observers.Contains(observer)) observers.Add(observer);
            subscribedConsoles++;
            UpdateSubscriptions();
            if(observers.Count == numberOfConsoles) notifyButton.interactable = true;
        }

        public void RemoveObserver(IObserver observer)
        {
            if (observers.Contains(observer)) observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach(IObserver observer in observers) observer.HandleNotification();
        }

        string GetStringNumber(int number)
        {
            switch (number)
            {
                case 0:
                    return "CERO";
                case 1:
                    return "UNO";
                case 2:
                    return "DOS";
                case 3:
                    return "TRES";
                case 4:
                    return "CUATRO";
            }

            return string.Empty;
        }
    }
}
