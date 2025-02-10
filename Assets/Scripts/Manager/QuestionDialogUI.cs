using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionDialogUI : SingletonA<QuestionDialogUI>
{
    [SerializeField] TextMeshProUGUI textMeshPro;
    [SerializeField] Button yesBtn;
    [SerializeField] Button noBtn;

    protected override void Awake()
    {
        base.Awake();
        Hide();
    }

    public void ShowQuestion(string questionText, Action yesAction, Action noAction)
    {
        gameObject.SetActive(true);

        textMeshPro.text = questionText;
        yesBtn.onClick.AddListener(() =>
        {
            Hide();
            yesAction();
        });
        noBtn.onClick.AddListener(() =>
        {
            Hide();
            noAction();
        });

    }

    public void Hide()
    {
        yesBtn.onClick.RemoveAllListeners();
        noBtn.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }
}

