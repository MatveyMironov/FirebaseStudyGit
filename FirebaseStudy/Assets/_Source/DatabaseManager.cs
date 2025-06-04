using Firebase.Database;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField hpInputField;
    [SerializeField] private TMP_InputField damageInputField;

    private string _userID;
    private DatabaseReference _reference;

    void Start()
    {
        _userID = SystemInfo.deviceUniqueIdentifier;
        _reference = FirebaseDatabase.DefaultInstance.RootReference;

        void SetHPInputFieldValue(object value) { SetInputFieldValue(hpInputField, value); }
        StartCoroutine(GetUserValue(SetHPInputFieldValue, "_hp"));

        void SetDamageInputFieldValue(object value) { SetInputFieldValue(damageInputField, value); }
        StartCoroutine(GetUserValue(SetDamageInputFieldValue, "_damage"));
    }

    public void CreateUser()
    {
        User user = new(int.Parse(hpInputField.text), int.Parse(damageInputField.text));
        string userJson = JsonUtility.ToJson(user);
        _reference.Child("users").Child(_userID).SetRawJsonValueAsync(userJson);
    }

    private IEnumerator GetUserValue(Action<object> onCallback, string valueName)
    {
        var userValueData = _reference.Child("users").Child(_userID).Child(valueName).GetValueAsync();

        yield return new WaitUntil(predicate: () => userValueData.IsCompleted);

        if (userValueData != null)
        {
            DataSnapshot dataSnapshot = userValueData.Result;

            onCallback.Invoke(dataSnapshot.Value);
        }
    }

    private void SetInputFieldValue(TMP_InputField inputField, object value)
    {
        inputField.text = value.ToString();
    }
}
