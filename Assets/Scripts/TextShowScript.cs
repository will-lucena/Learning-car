using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class TextShowScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textDisplayInfosGames;
    private string text;
    private float time = 0.0f;
    private int countListTimes = 0;
    private List<string> list = new List<string>(new string[] {
        "O veículo pilotado pelo jogador possui três sensores:",
        "Um em sua direita.",
        "Um em sua esquerda.",
        "Um emsua frente,",
        "Esses sensores servem para guardar dados da distância do carro as extremidades da pista.",
        "Esses valores são salvos em um arquivo juntamente com a velocidade angular do veículo, ou seja a direção que o veículo está seguindo.",
        "Quando adquirido um número alto de dados, esses são passadas para o treinamento com algoritmos de Machine learning para so então aplicar o modelo treinado á um veículo real(arduino).","" });
    private List<int> listTimes = new List<int>(new int[] {10, 5, 3, 3, 3, 7, 10, 12,1000});
    void Update()
    {
        time += Time.deltaTime;
        if (time > 52)
        {
            gameObject.SetActive(false);
        }
        else if(listTimes[countListTimes] < time)
        {
            textDisplayInfosGames.text = list[countListTimes];
            listTimes[countListTimes + 1] += listTimes[countListTimes];
            countListTimes++;
        }
        
    }
}
