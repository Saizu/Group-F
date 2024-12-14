using System;
using UnityEngine;

public class DateManager : MonoBehaviour
{
    void Start()
    {
        // 昨日の日付を取得するメソッドを呼び出し
        //string yesterdayDate = GetYesterdayDate(dateString);
    }

    public void ExtractDateParts(string date)
    {
        // ハイフンで分割
        string[] dateParts = date.Split('-');

        // 分割した部分を年、月、日に割り当て
        if (dateParts.Length == 3)
        {
            string year = dateParts[0];
            string month = dateParts[1];
            string day = dateParts[2];

            Debug.Log($"Year: {year}, Month: {month}, Day: {day}");
        }
        else
        {
            //Debug.LogError("Invalid date format. Ensure the string is in yyyy-MM-dd format.");
        }
    }

    public string GetYesterdayDate(string date)
    {
        try
        {
            // yyyy-MM-dd形式の文字列をDateTime型に変換
            DateTime dateTime = DateTime.ParseExact(date, "yyyy-MM-dd", null);

            // 昨日の日付を計算
            DateTime yesterday = dateTime.AddDays(-1);

            // yyyy-MM-dd形式の文字列に変換して返す
            return yesterday.ToString("yyyy-MM-dd");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error parsing date: {e.Message}");
            return null;
        }
    }
}
