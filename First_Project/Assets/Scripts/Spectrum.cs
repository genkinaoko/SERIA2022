using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class Spectrum : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
        // 空の Audio Sourceを取得
        var audio = GetComponent<AudioSource>();
        if ((audio != null)&&(Microphone.devices.Length>0)) // オーディオソースとマイクがある
        {
            string devName = Microphone.devices[0]; // 複数見つかってもとりあえず0番目のマイクを使用
            int minFreq, maxFreq;
            Microphone.GetDeviceCaps(devName, out minFreq, out maxFreq); // 最大最小サンプリング数を得る
            audio.clip = Microphone.Start(devName, true, 10, 44100); // 音の大きさを取るだけなので最小サンプリングで十分
            audio.Play(); //マイクをオーディオソースとして実行(Play)開始
        }

        // Audio Source の Audio Clip をマイク入力に設定
        // 引数は、デバイス名（null ならデフォルト）、ループ、何秒取るか、サンプリング周波数
        //audio.clip = Microphone.Start(null, false, 10, 44100);
        // マイクが Ready になるまで待機（一瞬）
        //while (Microphone.GetPosition(null) <= 0) {}
        // 再生開始（録った先から再生、スピーカーから出力するとハウリングします）
        //audio.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        var spectrum = GetComponent<AudioSource>().GetSpectrumData(2048, 0, FFTWindow.BlackmanHarris);
        var bun = "";
        for (int i = 1; i < spectrum.Length - 1; ++i) {
            Debug.DrawLine(
                    new Vector3(i - 1, spectrum[i] + 10, 0), 
                    new Vector3(i, spectrum[i + 1] + 10, 0), 
                    Color.red);
            Debug.DrawLine(
                    new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), 
                    new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), 
                    Color.black);

            Debug.DrawLine(
                    new Vector3(Mathf.Log(i - 1), spectrum[i - 1] + 10, 1), 
                    new Vector3(Mathf.Log(i), spectrum[i] + 10, 1), 
                    Color.green);
            Debug.DrawLine(
                    new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), 
                    new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), 
                    Color.yellow);
            bun = bun + spectrum[i].ToString() + "\n";
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            //Debug.Log(Mathf.Log(spectrum[4000]));
            //Debug.Log(spectrum.Length);
            Debug.Log(bun);
        }
    }
}