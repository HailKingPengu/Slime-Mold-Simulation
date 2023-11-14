using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Generate : MonoBehaviour
{

    [SerializeField] ComputeShader shader;
    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeField] public bool restart;
    [SerializeField] RenderTexture texture;

    [SerializeField] public int numberOfAgents;
    [SerializeField] public float agentSpeed;

    private Agent[] agents;
    [SerializeField] public float fadeAmount;
    [SerializeField] float blueShift;

    [SerializeField] public int blurSize;
    [SerializeField] public int blurSpeed;

    [SerializeField] public bool changeSpacingOverTime;
    [SerializeField] public float sensorAngleSpacing;
    [SerializeField] public float sensorOffsetDistance;
    [SerializeField] int sensorSize;
    [SerializeField] public float turnSpeed;

    [SerializeField] public bool useGradient;
    [SerializeField] public Color agentColour;
    [SerializeField] Gradient colourGradient;

    struct Agent
    {
        Vector2 position;
        float angle;
        public Agent(Vector2 pos, float ang)
        {
            position = pos;
            angle = ang;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        width = Screen.width;
        height = Screen.height;

        texture = new RenderTexture(width, height, 24);
        texture.enableRandomWrite = true;
        texture.Create();

        int kernel = shader.FindKernel("Update");
        shader.SetTexture(kernel, "Result", texture);
        AddAgents(numberOfAgents);

        texture = new RenderTexture(width, height, 24);
        texture.enableRandomWrite = true;
        texture.Create();

    }

    // Update is called once per frame
    void Update()
    {
        //if (generate)
        //{
        //    shader.SetTexture(0, "Result", texture);
        //    shader.Dispatch(0, texture.width / 8, texture.height / 8, 1);
        //    generate = false;
        //}
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        //texture = new RenderTexture(width, height, 24);
        //texture.enableRandomWrite = true;
        //texture.Create();

        //if(width != Screen.width || height != Screen.height)
        //{
        //    width = Screen.width;
        //    texture.width = width;

        //    height = Screen.height;
        //    texture.height = height;
        //}

        if (changeSpacingOverTime)
        {
            sensorAngleSpacing = 14 + 12 * Mathf.Sin(0.005f * Time.time);
        }

        if (useGradient)
        {
            agentColour = colourGradient.Evaluate((0.05f * Time.time) % 1);
        }

        //Debug.Log((0.05f * Time.time) % 1);

        int kernel = shader.FindKernel("Update");

        if (restart)
        {
            AddAgents(numberOfAgents);
        }
        restart = false;

        shader.SetFloat("sensorAngleSpacing", sensorAngleSpacing);
        shader.SetFloat("sensorOffsetDistance", sensorOffsetDistance);
        shader.SetInt("sensorSize", sensorSize);
        shader.SetFloat("turnSpeed", turnSpeed);

        shader.SetFloats("agentColour", new float[] {agentColour.r, agentColour.g, agentColour.b, agentColour.a});

        shader.SetFloat("moveSpeed", agentSpeed);
        shader.SetTexture(kernel, "Result", texture);
        shader.SetInt("width", width);
        shader.SetInt("height", height);
        //shader.Dispatch(0, texture.width / 8, texture.height / 8, 1);
        shader.Dispatch(kernel, agents.Length / 256, 1, 1);

        int fadeKernel = shader.FindKernel("Fade");
        shader.SetFloat("fadeAmount", fadeAmount);
        shader.SetFloat("blueShift", blueShift);
        shader.SetTexture(fadeKernel, "Result", texture);
        shader.Dispatch(fadeKernel, width / 8 + 1 , height / 8 + 1, 1);

        int blurKernel = shader.FindKernel("Blur");
        shader.SetTexture(blurKernel, "Result", texture);
        shader.SetInt("blurSize", blurSize);
        shader.SetFloat("blurSpeed", blurSpeed);
        shader.Dispatch(blurKernel, width / 8 + 1, height / 8 + 1, 1);

        Graphics.Blit(texture, destination);
    }

    private void AddAgents(int num)
    {

        agents = new Agent[num];

        for(int i = 0; i < num; i++)
        {
            agents[i] = new Agent(new Vector2(width / 2, height / 2), Random.Range(0f, 360f));
        }

        int posSize = sizeof(float) * 2;
        int angleSize = sizeof(float);
        int totalSize = posSize + angleSize;

        ComputeBuffer agentBuffer = new ComputeBuffer(num, totalSize);
        agentBuffer.SetData(agents);

        int kernel = shader.FindKernel("Update");
        shader.SetInt("numAgents", num);
        shader.SetFloat("deltaTime", Time.deltaTime);
        shader.SetBuffer(kernel, "agents", agentBuffer);
        shader.Dispatch(kernel, num / 256, 1, 1);

        //agentBuffer.Dispose();
    }

    //private void LoadAgents()
}
