using UnityEngine;
using System.Collections;
//using UnityEngine.UI;

public class KartScript : MonoBehaviour
{
    public string Nome;
    public int contProgresso;
    private int AuxContProg;
    public int lap = 0;
    public float velAtual;
    public int posicao;
    public bool Terminou = false;
    public string tamanhoPersonagem = "Grande";
    public Transform PosFrente, PosTras;
    public Transform CenterOfMass;
    public Rigidbody KartRigidbody;
    private float tempo = 0;
    public int minutos = 0, segundos = 0;

    #region Prefabs PowerUp
    public Object AranhaExplosivaPrefab;
    public Object PowerUpBoxFalsa;
    public Rigidbody MisselPrefab;
    public Rigidbody MisselGuiadoPrefab;
    public Object OleoPrefab;
    #endregion

    #region Rodas
    //Declaração das meshs das rodas
    public Transform RodaFDirMesh;
    public Transform RodaFEsqMesh;
    public Transform RodaTDirMesh;
    public Transform RodaTEsqMesh;

    //Declaração dos colisores das rodas
    public WheelCollider RodaFDir;
    public WheelCollider RodaFEsq;
    public WheelCollider RodaTDir;
    public WheelCollider RodaTEsq;
    #endregion

    #region variáveis privadas
    private AranhaExplosivaScript AranhaScript;
    private GameObject ultimoCheckpoint;
    private Vector3 posInicial;
    private Quaternion rotInicial;
    private float velMax, auxVel, angAtual;
    private float aceleracao, velDesaceleracao;
    private float direcaoBaixaVel, direcaoAltaVel;
    private float direcaoBaixaVelDrift, direcaoAltaVelDrift;
    private float friccaoLateralTras, friccaoFrontalTras, friccaoLateralFrente, friccaoFrontalFrente; //Friccao normal das rodas
    private float friccaoLateralDriftTras, friccaoFrontalDriftTras, friccaoLateralDriftFrente, friccaoFrontalDriftFrente;  //Friccao das rodas no drift
    private float auxRotX, auxRotZ;
    private float countEspecial, CooldownEspecial;
    private bool jaContou = false;
    private bool especialDisponivel = false;
    public int powerUpTipo = 0;
    private Vector3 PowerUpPosition;
    private Quaternion PowerUpRotation;
    private bool boost = false;
    private float contBoost = 0;
    private float contTravou = 0;
    public bool Jogando = false;
    public int ContCP = 0;
    #endregion

    void Start()
    {
        KartRigidbody = GetComponent<Rigidbody>(); //Identifica e pega a referencia do rigidbody do carro 
        KartRigidbody.centerOfMass = new Vector3(CenterOfMass.localPosition.x * transform.localScale.x,   //Ajusta o centro de massa do Kart
                                                 CenterOfMass.localPosition.y * transform.localScale.y - 0.003f,
                                                 CenterOfMass.localPosition.z * transform.localScale.z - 0.005f);
        posInicial = KartRigidbody.position; //Salva sua posição inicial na corrida
        rotInicial = KartRigidbody.rotation; //Salva sua rotação inicial na corrida
        ArmazenaFriccao();       //Armazena a friccao inicial das rodas e configura a friccao do drift
        PersonalizaPersonagem(); //Ajusta atributos de acordo com o tipo de personagem
        CooldownEspecial = 60;   //Define o tempo de recarga do powerup especial   
    }

    void Update()
    {
        if (Jogando)
        {
            movimentarRodas(); //Movimenta as meshes de acordo com os colliders
            recargaEspecial(); //Conta o tempo de recarga do powerup especial
            VerificaTravado(); //Verifica se o kart não está travado em algum lugar da pista
            Timer();           //Inicia o timer
        }
    }

    #region Funções de Movimentação

    public void Velocimetro()
    {
        //Armazena a velocidade atual.
        auxVel = KartRigidbody.velocity.magnitude / (2 * velMax);
        velAtual = 2 * 22 / 7 * RodaFDir.radius * RodaFDir.rpm * 60 / 100000;
        velAtual = Mathf.Round(velAtual);
    }

    public void ExecutarDrift(bool drift)
    {
        if (drift) //Está no drift
        {
            if (velAtual > 0)
            {
                #region Altera fricção enquanto está no drift
                ConfigurarFriccao(RodaFDir, friccaoFrontalDriftFrente, friccaoLateralDriftFrente);
                ConfigurarFriccao(RodaFEsq, friccaoFrontalDriftFrente, friccaoLateralDriftFrente);
                ConfigurarFriccao(RodaTDir, friccaoFrontalDriftTras, friccaoLateralDriftTras);
                ConfigurarFriccao(RodaTEsq, friccaoFrontalDriftTras, friccaoLateralDriftTras);
                #endregion
            }
            else
            {
                #region Volta fricção pro valor normal
                ConfigurarFriccao(RodaFDir, friccaoFrontalFrente, friccaoLateralFrente);
                ConfigurarFriccao(RodaFEsq, friccaoFrontalFrente, friccaoLateralFrente);
                ConfigurarFriccao(RodaTDir, friccaoFrontalTras, friccaoLateralTras);
                ConfigurarFriccao(RodaTEsq, friccaoFrontalTras, friccaoLateralTras);
                #endregion
            }
        }
        else //Não tá no drift
        {
            #region Volta fricção pro valor normal
            ConfigurarFriccao(RodaFDir, friccaoFrontalFrente, friccaoLateralFrente);
            ConfigurarFriccao(RodaFEsq, friccaoFrontalFrente, friccaoLateralFrente);
            ConfigurarFriccao(RodaTDir, friccaoFrontalTras, friccaoLateralTras);
            ConfigurarFriccao(RodaTEsq, friccaoFrontalTras, friccaoLateralTras);
            #endregion
        }
    }

    public void Acelerar(float direção)
    {
       
        #region Zerar o freio das rodas caso estiver freiando
        if (RodaTDir.brakeTorque > 0)
        {
            RodaTDir.brakeTorque = 0;
            RodaTEsq.brakeTorque = 0;
            RodaFDir.brakeTorque = 0;
            RodaFEsq.brakeTorque = 0;

            RodaTDir.motorTorque = 0.00001f;
            RodaTEsq.motorTorque = 0.00001f;
            RodaFDir.motorTorque = 0.00001f;
            RodaFEsq.motorTorque = 0.00001f;
        }
        #endregion

        #region Acelerar
        if (velAtual <= velMax) //Se a vel está abaixo da máxima, pode acelerar. Senão, para de acelerar.
        {
            if (boost)
            {
                #region Aceleração quando boost está ativado
                RodaTDir.motorTorque = aceleracao * 500f * direção;
                RodaTEsq.motorTorque = aceleracao * 500f * direção;
                if (contBoost <= 25)
                {
                    contBoost += Time.deltaTime/Time.timeScale;
                }
                else
                {
                    boost = false;
                    contBoost = 0;
                }
                #endregion
            }
            else
            {
                #region Aceleração normal, sem boost
                RodaTDir.motorTorque = aceleracao * direção;
                RodaTEsq.motorTorque = aceleracao * direção;
                RodaFDir.motorTorque = aceleracao * direção; //Se tirar a aceleração das rodas frontais o kart não anda
                RodaFEsq.motorTorque = aceleracao * direção;
                #endregion
            }
        }
        else // Velocidade acima do permitido, zera a aceleração do motor.
        {
            RodaTDir.motorTorque = 0.00001f;
            RodaTEsq.motorTorque = 0.00001f;
            RodaFDir.motorTorque = 0.00001f;
            RodaFEsq.motorTorque = 0.00001f;
        }
        #endregion

        
    }

    public void Desacelerar()
    {
        RodaTDir.brakeTorque = velDesaceleracao;
        RodaTEsq.brakeTorque = velDesaceleracao;
        //RodaFDir.brakeTorque = velDesaceleracao;  //Modificar
        //RodaFEsq.brakeTorque = velDesaceleracao;
    }

    public void Direção(float direção, bool drift)
    {
        #region Direção do kart
        if (drift)
        {
            angAtual = Mathf.Lerp(direcaoBaixaVelDrift, direcaoAltaVelDrift, auxVel);
            RodaFDir.steerAngle = -angAtual;
            RodaFEsq.steerAngle = -angAtual;
            RodaTDir.steerAngle = angAtual * 0.2f;
            RodaTEsq.steerAngle = angAtual * 0.2f;
        }
        else
        {
            angAtual = Mathf.Lerp(direcaoBaixaVel, direcaoAltaVel, auxVel);
            angAtual *= direção;
            RodaFDir.steerAngle = angAtual;
            RodaFEsq.steerAngle = angAtual;
            RodaTDir.steerAngle = 0f;
            RodaTEsq.steerAngle = 0f;
        }
        #endregion
    }

    public void Direção(float rotaçãoAI)
    {
        RodaFEsq.steerAngle = rotaçãoAI;
        RodaFDir.steerAngle = rotaçãoAI;
    }

    #endregion

    #region Funções Públicas

    public void voltarNoCheckpoint() //Faz o jogador voltar no ultimo checkpoint coletado
    {
        if (ultimoCheckpoint != null)
        {
            KartRigidbody.position = ultimoCheckpoint.transform.position;
            KartRigidbody.rotation = ultimoCheckpoint.transform.rotation;
        }
        else
        {
            KartRigidbody.position = posInicial;
            KartRigidbody.rotation = rotInicial;
        }
        contTravou = 0;
    }

    public void Rodar() //Jogador foi atingido
    {
       // Destroy(this.gameObject);
    }

    #endregion

    #region Funções Privadas

    private void ArmazenaFriccao()
    {
        #region Atrito das Rodas
        friccaoLateralTras = RodaTDir.sidewaysFriction.stiffness;     //Armazena a fricção lateral normal das rodas de trás
        friccaoFrontalTras = RodaTDir.forwardFriction.stiffness;      //Armazena a fricção frontal normal das rodas de trás
        friccaoLateralFrente = RodaFDir.sidewaysFriction.stiffness;   //Armazena a fricção lateral normal das rodas da frente
        friccaoFrontalFrente = RodaFDir.forwardFriction.stiffness;    //Armazena a fricção frontal normal das rodas da frente
        friccaoLateralDriftTras = 0.5f;                               //Armazena a fricção lateral das rodas de trás no drift
        friccaoFrontalDriftTras = 0.9f;                               //Armazena a fricção frontal das rodas de trás no drift
        friccaoLateralDriftFrente = 0.5f;                             //Armazena a fricção lateral das rodas da frente no drift
        friccaoFrontalDriftFrente = 0.9f;                             //Armazena a fricção frontal das rodas da frente no drift
        #endregion
    }

    private IEnumerator Delay(int segundos, string tipo)
    {
        yield return new WaitForSeconds(segundos);
        switch (tipo)
        {
            case "colisão":
                {
                    jaContou = false;
                }
                break;
        }
    }

    private void Timer()
    {
        if (!Terminou)
        tempo += Time.deltaTime/Time.timeScale;
        segundos = (int) Mathf.Round(tempo);
        minutos = segundos / 60;
        segundos = segundos - (60 * minutos);
    }

    private void movimentarRodas() //Movimento das meshs das rodas
    {
        //Faz as rodas rodarem conforme o carro anda
        RodaFDirMesh.Rotate(RodaFDir.rpm * (60f * 360f * Time.deltaTime * Time.timeScale), 0, 0);
        RodaFEsqMesh.Rotate(RodaFEsq.rpm * (60f * 360f * Time.deltaTime * Time.timeScale), 0, 0);
        RodaTDirMesh.Rotate(RodaTDir.rpm * (60f * 360f * Time.deltaTime * Time.timeScale), 0, 0);
        RodaTEsqMesh.Rotate(RodaTEsq.rpm * (60f * 360f * Time.deltaTime * Time.timeScale), 0, 0);

        //Faz as rodas mudarem de posição de acordo com a suspensão
        //RodaFDirMesh.transform.position = RodaFDir.transform.position - new Vector3(0, (RodaFDir.radius), 0);
        //RodaFEsqMesh.transform.position = RodaFEsq.transform.position - new Vector3(0, (RodaFEsq.radius), 0);
        //RodaTDirMesh.transform.position = RodaTDir.transform.position - new Vector3(0, (RodaTDir.radius), 0);
        //RodaTEsqMesh.transform.position = RodaTEsq.transform.position - new Vector3(0, (RodaTEsq.radius), 0);
      
        //Faz as rodas dianteiras virarem quando o volante vira
        auxRotX = RodaFDirMesh.localEulerAngles.x;
        auxRotZ = RodaFDirMesh.localEulerAngles.z;
        RodaFDirMesh.localEulerAngles = new Vector3(auxRotX, RodaFDir.steerAngle, auxRotZ);
        auxRotX = RodaFEsqMesh.localEulerAngles.x;
        auxRotZ = RodaFEsqMesh.localEulerAngles.z;
        RodaFEsqMesh.localEulerAngles = new Vector3(auxRotX, RodaFEsq.steerAngle, auxRotZ);

        //Faz as rodas traseiras virarem quando o volante vira      
        auxRotX = RodaTDirMesh.localEulerAngles.x;
        auxRotZ = RodaTDirMesh.localEulerAngles.z;
        RodaTDirMesh.localEulerAngles = new Vector3(auxRotX, RodaTDir.steerAngle, auxRotZ);
        auxRotX = RodaTEsqMesh.localEulerAngles.x;
        auxRotZ = RodaTEsqMesh.localEulerAngles.z;
        RodaTEsqMesh.localEulerAngles = new Vector3(auxRotX, RodaTEsq.steerAngle, auxRotZ);
    }

    private void PersonalizaPersonagem()
    {
        switch (tamanhoPersonagem) //Personalização de acordo com o tamanho do personagem
        {
            #region Personagem Pequeno
            case "Pequeno":
                {
                    velMax = 10000000000000000;
                    aceleracao = 200;
                    direcaoBaixaVel = 50f;
                    direcaoAltaVel = 45f;
                }
                break;
            #endregion

            #region Personagem Médio
            case "Médio":
                {
                    velMax = 10000000000000000;
                    aceleracao = 180;
                    direcaoBaixaVel = 55f;
                    direcaoAltaVel = 50f;
                }
                break;
            #endregion

            #region Personagem Grande
            case "Grande":
                {
                    velMax = 10000000000000000;
                    aceleracao = 150;
                    direcaoBaixaVel = 65f;
                    direcaoAltaVel = 55f;
                }
                break;
            #endregion
        }

        //Repassa a personalização do tamanho pra outras propriedades
        velDesaceleracao = aceleracao * 0.09f;                   //Configura o valor da vel de desaceleração
        direcaoBaixaVelDrift = direcaoBaixaVel * 1.25f;         //Configura o valor sensibilidade da direção no drift
        direcaoAltaVelDrift = direcaoAltaVel * 1.25f;           //Configura o valor sensibilidade da direção no drift
    }

    private void ConfigurarFriccao(WheelCollider Roda, float friccaoFrontalAtual, float friccaoLateralAtual)
    {
        WheelFrictionCurve auxFoward, auxSideway;
        //Roda que é passada por parametro é modificada
        auxFoward = Roda.forwardFriction;
        auxSideway = Roda.sidewaysFriction;
        auxFoward.stiffness = friccaoFrontalAtual;
        auxSideway.stiffness = friccaoLateralAtual;
        Roda.forwardFriction = auxFoward;
        Roda.sidewaysFriction = auxSideway;
    }

    private void recargaEspecial()
    {
        #region Tempo de Recarga PowerUp Especial
        if (countEspecial >= CooldownEspecial)
        {
            especialDisponivel = true;
        }
        else
        {
            countEspecial += Time.deltaTime/Time.timeScale;
            especialDisponivel = false;
        }
        #endregion
    }

    private void ajustarDireçãoPowerUp(string direção)
    {
        if (direção == "Trás")
        {
            PowerUpPosition = PosTras.position;
            PowerUpRotation = PosTras.rotation;
        }
        else
        {
            PowerUpPosition = PosFrente.position;
            PowerUpRotation = KartRigidbody.rotation;
        }
    }

    private void VerificaTravado() //Verifica se o jogador está travado/parado a mais de 5 segundos
    {
        if (KartRigidbody.velocity.magnitude < 1)
        {
            contTravou += Time.deltaTime / Time.timeScale;
            if (contTravou > 5)
                voltarNoCheckpoint();
        }
        else
        {
            contTravou = 0;
        }
    }

    #endregion

    #region Funções de PowerUp

    public void powerUpComum(string direção)
    {
        switch (powerUpTipo)
        {
            #region Missel Comum
            case 1: //Missel Comum
                {
                    ajustarDireçãoPowerUp(direção);
                    Instantiate(MisselPrefab, PowerUpPosition, PowerUpRotation);
                    powerUpTipo = 0;
                }
                break;
            #endregion

            #region Boost de Velocidade
            case 2: //Boost de Velocidade
                {
                   // boost = true;
                    powerUpTipo = 0;
                }
                break;
            #endregion

            #region Missel Teleguiado
            case 3: //Missel Teleguiado
                {
                    ajustarDireçãoPowerUp(direção);
                    Instantiate(MisselGuiadoPrefab, PowerUpPosition, PowerUpRotation);
                    powerUpTipo = 0;
                }
                break;
            #endregion

            #region Oleo
            case 4:  //Oleo
                {
                    Instantiate(OleoPrefab, PosTras.position, PosTras.rotation);
                    powerUpTipo = 0;
                }
                break;
            #endregion

            #region Power Up Box Falsa
            case 5: //Power Up Box Falsa
                {
                    Instantiate(PowerUpBoxFalsa, PosTras.position, PosTras.rotation);
                    powerUpTipo = 0;
                }
                break;
            #endregion

            #region Aranha Explosiva
            /*case 6: //Aranha Explosiva
                {
                    Instantiate(AranhaExplosivaPrefab, PosTras.position, PosTras.rotation);
                    powerUpTipo = 0;
                }
                break;*/
            #endregion
        }
    }

    public void powerUpEspecial()
    {
        if (especialDisponivel)
        {
            switch (this.gameObject.name)
            {
                case "Violetta":
                    {

                    }
                    break;
                case "Jeshi":
                    {

                    }
                    break;
                case "Momoto":
                    {

                    }
                    break;
                case "Ayah":
                    {

                    }
                    break;
            }
            countEspecial = 0; //Inicia novamente a contagem do tempo de recarga
        }

    }

    #endregion

    #region Triggers

    private void OnTriggerExit(Collider Objeto)
    {
        if (Objeto.gameObject.CompareTag("AranhaAoE"))
        {
            AranhaScript = Objeto.transform.parent.gameObject.GetComponent<AranhaExplosivaScript>();
            AranhaScript.removeAlvo = this.gameObject;
        }
    }

    private void OnTriggerEnter(Collider Objeto)
    {
        #region Area de Explosão da Aranha
        if (Objeto.gameObject.CompareTag("AranhaAoE"))
        {
            AranhaScript = Objeto.transform.parent.gameObject.GetComponent<AranhaExplosivaScript>();
            AranhaScript.addAlvo = this.gameObject;
        }
        #endregion
        #region Limite de Pista
        if (Objeto.gameObject.CompareTag("Limite"))
        {
            voltarNoCheckpoint();
        }
        #endregion
        #region Checkpoints
        if (Objeto.gameObject.CompareTag("Checkpoint"))
        {
            AuxContProg = Objeto.gameObject.GetComponent<CheckpointScript>().Num;
            if (ultimoCheckpoint == null)
            {
                contProgresso = AuxContProg;
                ultimoCheckpoint = Objeto.gameObject;
                ContCP++;
            }
            else if (ultimoCheckpoint != Objeto.gameObject)
            {
                ultimoCheckpoint = Objeto.gameObject;
                contProgresso = AuxContProg;
                ContCP++;
            }
        }
        #endregion
        #region Contador de Voltas
        if (Objeto.gameObject.CompareTag("LapCounter"))
        {
            if (jaContou)
            {
                StartCoroutine(Delay(30, "colisão"));
            }
            else
            {
                lap++;
                jaContou = true;
            }
        }
        #endregion
        #region Randomização de PowerUps Comuns
        if (Objeto.gameObject.CompareTag("PowerUpBox"))
        {
            powerUpTipo = Random.Range(1, 6);
        }
        #endregion
    }

    #endregion

}
