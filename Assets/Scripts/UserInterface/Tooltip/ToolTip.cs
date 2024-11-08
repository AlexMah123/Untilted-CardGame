 using System;
 using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.Tooltip
{
    [ExecuteInEditMode]
    public class ToolTip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI headerField;
        [SerializeField] private TextMeshProUGUI contentField;
        [SerializeField] private TextMeshProUGUI inspectField;
        [SerializeField] private TextMeshProUGUI reminderField;
        [SerializeField] private LayoutElement layoutElement;


        private void Awake()
        {
            if (headerField == null || contentField == null || reminderField == null)
            {
                Debug.LogError("Tooltip field and/or reminder field are missing!");
            }
        }

        public void SetText(string content, string header = "", bool displayInspect = true, bool displayReminder = true)
        {
            //display header if not empty
            if (string.IsNullOrEmpty(header))
            {
                headerField.gameObject.SetActive(false);
            }
            else
            {
                headerField.gameObject.SetActive(true);
                headerField.text = header;
            }
            
            //display content and reminder field
            contentField.text = content;
            inspectField.gameObject.SetActive(displayInspect);
            reminderField.gameObject.SetActive(displayReminder);

            
            //if either header or content is bigger, than layoutElement, enable
            layoutElement.enabled = Mathf.Max(headerField.preferredWidth, contentField.preferredWidth) >= layoutElement.preferredWidth;
        }
        
        private void LateUpdate()
        {
            transform.position = Input.mousePosition;
        }
    }
}