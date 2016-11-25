using UnityEngine;
using System.Collections;
//using UnityEngine.UI;

public class KartScript : MonoBehaviour
{
    public Camera CamKart;
    private KartCameraScript ScriptCam;
    public AudioSource Audio;
    public InterfaceScript Interface;
    public string Nome;
    public Animator Animacao;
    public float AnimSide, AnimFront, TempoProvocacao;
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
    public int minutos = 0, segundos = 0, milisegundos = 0;
    public float AuxMilisegundos = 0;
    private bool lento = false;
    private float contLento = 0;
    private float contImunidade;
    private bool imune = false;
    private GameObject KartAlvo;
    private int colocacaoAlvo;
    public int ProgNoFim = 0;
    private bool PegouPUEspecial;
    public bool SomouTempoExtra = false;
    private bool AplicandoEfeitoCam = false;
    private bool AplicandoEfeitoGlitch = false;

    #region Prefabs PowerUp
    public Object AranhaExplosivaPrefab;
    public Object PowerUpBoxFalsa;
    public Rigidbody MisselPrefab;
    public Rigidbody MisselGuiadoPrefab;
    public Object OleoPrefab;
    public Object PowerUpEspecial;
    public GameObject PowerUpEspecialAyah;
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

    #region Variáveis privadas
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
    private float contTravou = 0, contTravou2 = 0;
    private bool FazendoEfeitoColidir;
    public bool Jogando = false;
    public int ContCP = 0;
    private bool deixaRastro = false;
    private float contRastro = 0;
    #endregion

    public AudioClip Dano, PegaPowerUp, Andando, Explosao;
    public float VolumeDano, VolumePegaPowerUp, VolumeAndando, VolumeExplosao;
    public ParticleSystem Rastro, LevouDano, PegouPowerUp, ExplosaoMissel, Ganhou;
    public UnityStandardAssets.ImageEffects.GlitchEffect EfeitoGlitch;

    void Start()
    {
        ScriptCam = CamKart.GetComponent<KartCameraScript>();
        KartRigidbody = GetComponent<Rigidbody>(); //Identifica e pega a referencia do rigidbody do carro 
        KartRigidbody.centerOfMass = new Vector3(CenterOfMass.localPosition.x * transform.localScale.x,   //Ajusta o centro de massa do Kart
                                                 CenterOfMass.localPosition.y * transform.localScale.y ,
                                                 CenterOfMass.localPosition.z * transform.localScale.z );
        posInicial = KartRigidbody.position; //Salva sua posição inicial na corrida
        rotInicial = KartRigidbody.rotation; //Salva sua rotação inicial na corrida
        ArmazenaFriccao();       //Armazena a friccao inicial das rodas e configura a friccao do drift
        PersonalizaPersonagem(); //Ajusta atributos de acordo com o tipo de personagem
        CooldownEspecial = 60;   //Define o tempo de recarga do powerup especial  
        if (Andando != null)
            Audio.PlayOneShot(Andando, VolumeAndando);
    }

    void Update()
    {
        if (Terminou)
            Animacao.SetBool("Comemoração", true);
        else
            Animacao.SetBool("Comemoração", false);

        if (ScriptCam.cameraReversa)      
            Animacao.SetBool("OlhandoParaTras", true);
        else
            Animacao.SetBool("OlhandoParaTras", false);

        if (Jogando)
        {            
            Peso();
            movimentarRodas(); //Movimenta as meshes de acordo com os colliders
            recargaEspecial(); //Conta o tempo de recarga do powerup especial
            VerificaTravado(); //Verifica se o kart não está travado em algum lugar da pista
            Timer();           //Inicia o timer
            Lentidao();        //Deixa o kart lento caso seja atingido
            if (PowerUpEspecialAyah != null)
                Imunidade();
            if (deixaRastro)
                RastroPedras();
            Interface.setPowerUp(powerUpTipo);
            Interface.setPowerUpEspecial(especialDisponivel);
            ParticulasRastroFumaca();
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
            #region Aceleração normal, sem boost
            RodaTDir.motorTorque = aceleracao*2 * direção;
            RodaTEsq.motorTorque = aceleracao*2 * direção;
           // RodaFDir.motorTorque = aceleracao * direção; //Se tirar a aceleração das rodas frontais o kart não anda
           // RodaFEsq.motorTorque = aceleracao * direção;
            #endregion

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
            //angAtual = Mathf.Lerp(direcaoBaixaVel, direcaoAltaVel, auxVel);
            //angAtual *= direção;
            if (direção > 0)
                Animacao.SetFloat("side", -AnimSide);
            if (direção < 0)
                Animacao.SetFloat("side", AnimSide);
            if (direção == 0)
                Animacao.SetFloat("side", 0);

            RodaFDir.steerAngle = direção * 25;
            RodaFEsq.steerAngle = direção * 25;
            RodaTDir.steerAngle = 0f;
            RodaTEsq.steerAngle = 0f;
        }
        #endregion
    }

    public void Direção(float rotaçãoAI)
    {
        if (rotaçãoAI > 0)
            Animacao.SetFloat("side", -AnimSide);
        if (rotaçãoAI < 0)
            Animacao.SetFloat("side", AnimSide);
        if (rotaçãoAI == 0)
            Animacao.SetFloat("side", 0);
        RodaFEsq.steerAngle = rotaçãoAI;
        RodaFDir.steerAngle = rotaçãoAI;
    }

    #endregion

    #region Funções Públicas

    public void SomaTempoExtra(float TempoExtra)
    {
        if (!SomouTempoExtra)
        {
            tempo += TempoExtra;
            AuxMilisegundos = tempo % 1f;
            segundos = (int)Mathf.Round(tempo);
            minutos = segundos / 60;
            segundos = segundos - (60 * minutos);
            milisegundos = (int)Mathf.Round(AuxMilisegundos * 1000);
            SomouTempoExtra = true;
        }
    }


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

    private void Peso()
    {
        KartRigidbody.AddForceAtPosition((-5 * KartRigidbody.velocity.sqrMagnitude) * transform.up, transform.position);
    }

    private void ArmazenaFriccao()
    {
        #region Atrito das Rodas
        friccaoLateralTras = RodaTDir.sidewaysFriction.stiffness;     //Armazena a fricção lateral normal das rodas de trás
        friccaoFrontalTras = RodaTDir.forwardFriction.stiffness;      //Armazena a fricção frontal normal das rodas de trás
        friccaoLateralFrente = RodaFDir.sidewaysFriction.stiffness;   //Armazena a fricção lateral normal das rodas da frente
        friccaoFrontalFrente = RodaFDir.forwardFriction.stiffness;    //Armazena a fricção frontal normal das rodas da frente
        friccaoLateralDriftTras = 0.8f;                               //Armazena a fricção lateral das rodas de trás no drift
        friccaoFrontalDriftTras = 0.3f;                               //Armazena a fricção frontal das rodas de trás no drift
        friccaoLateralDriftFrente = 0.8f;                             //Armazena a fricção lateral das rodas da frente no drift
        friccaoFrontalDriftFrente = 0.3f;                             //Armazena a fricção frontal das rodas da frente no drift
        #endregion
    }

    private IEnumerator DelayLapCounter()
    {
        yield return new WaitForSeconds(60);
        jaContou = false;

    }

    private IEnumerator EfeitoCamera()
    {
        Animacao.SetFloat("front", -AnimFront);
        for (int i = 1; i <= 20; i++)
        {
            CamKart.fieldOfView = 60 + i;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1);
        for (int i=1;i<=20;i++)
        {
            CamKart.fieldOfView = 80 - i;
            yield return new WaitForSeconds(0.01f);
        }
        CamKart.fieldOfView = 60;

        Animacao.SetFloat("front", 0);
        AplicandoEfeitoCam = false;
    }

    private void Timer()
    {
        if (!Terminou)
        tempo += Time.deltaTime/Time.timeScale;
        AuxMilisegundos = tempo % 1f;
        segundos = (int) Mathf.Round(tempo);
        minutos = segundos / 60;
        segundos = segundos - (60 * minutos);
        milisegundos = (int)Mathf.Round(AuxMilisegundos * 1000);
    }

    private void movimentarRodas() //Movimento das meshs das rodas
    {
		Vector3 pos;
		Quaternion rot;
		RodaFDir.GetWorldPose (out pos, out rot);
		//RodaFDirMesh.transform.position = pos;
		RodaFDirMesh.transform.rotation = rot;

		RodaFEsq.GetWorldPose (out pos, out rot);
		//RodaFEsqMesh.transform.position = pos;
		RodaFEsqMesh.transform.rotation = rot;

		RodaTDir.GetWorldPose (out pos, out rot);
		//RodaTDirMesh.transform.position = pos;
		RodaTDirMesh.transform.rotation = rot;

		RodaTEsq.GetWorldPose (out pos, out rot);
		//RodaTEsqMesh.transform.position = pos;
		RodaTEsqMesh.transform.rotation = rot;


       /* //Faz as rodas rodarem conforme o carro anda
      // RodaFDirMesh.Rotate(RodaFDir.rpm * (60f * 360f * Time.deltaTime * Time.timeScale), 0, 0);
        RodaFEsqMesh.Rotate(RodaFEsq.rpm * (60f * 360f * Time.deltaTime * Time.timeScale), 0, 0);
        RodaTDirMesh.Rotate(RodaTDir.rpm * (60f * 360f * Time.deltaTime * Time.timeScale), 0, 0);
        RodaTEsqMesh.Rotate(RodaTEsq.rpm * (60f * 360f * Time.deltaTime * Time.timeScale), 0, 0);
              
        //Faz as rodas dianteiras virarem quando o volante vira
        auxRotX = RodaFDirMesh.localEulerAngles.x;
        auxRotZ = RodaFDirMesh.localEulerAngles.z;
        //RodaFDirMesh.localEulerAngles = new Vector3(auxRotX, RodaFDir.steerAngle, auxRotZ);
        auxRotX = RodaFEsqMesh.localEulerAngles.x;
        auxRotZ = RodaFEsqMesh.localEulerAngles.z;
        RodaFEsqMesh.localEulerAngles = new Vector3(auxRotX, RodaFEsq.steerAngle, auxRotZ);



        //Faz as rodas mudarem de posição de acordo com a suspensão
        //RodaFDirMesh.transform.position = RodaFDir.transform.position - new Vector3(0, (RodaFDir.radius), 0);
        //RodaFEsqMesh.transform.position = RodaFEsq.transform.position - new Vector3(0, (RodaFEsq.radius), 0);
        //RodaTDirMesh.transform.position = RodaTDir.transform.position - new Vector3(0, (RodaTDir.radius), 0);
        //RodaTEsqMesh.transform.position = RodaTEsq.transform.position - new Vector3(0, (RodaTEsq.radius), 0);

        /*
        //Faz as rodas traseiras virarem quando o volante vira      
        auxRotX = RodaTDirMesh.localEulerAngles.x;
        auxRotZ = RodaTDirMesh.localEulerAngles.z;
        RodaTDirMesh.localEulerAngles = new Vector3(auxRotX, RodaTDir.steerAngle, auxRotZ);
        auxRotX = RodaTEsqMesh.localEulerAngles.x;
        auxRotZ = RodaTEsqMesh.localEulerAngles.z;
        RodaTEsqMesh.localEulerAngles = new Vector3(auxRotX, RodaTEsq.steerAngle, auxRotZ);*/
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
                    aceleracao = 170;
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
            if (!PegouPUEspecial)
            {
                PegouPowerUp.Play();
                PegouPUEspecial = true;
            }
        }
        else
        {
            countEspecial += Time.deltaTime/Time.timeScale;
            especialDisponivel = false;
            PegouPUEspecial = false;
        }
        #endregion
    }

    private GameObject buscarAlvo(string direção)
    {        
        if (direção == "Trás")
            colocacaoAlvo = posicao - 1;
        else
            colocacaoAlvo = posicao + 1;

        return GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>().BuscarKartAlvo(colocacaoAlvo);          
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
        if (!RodaFDir.isGrounded && !RodaFEsq.isGrounded && !RodaTDir.isGrounded && !RodaTEsq.isGrounded)
        {
            contTravou2 += Time.deltaTime / Time.timeScale;
            if (contTravou2 > 2)
                voltarNoCheckpoint();
        }
        else
        {
            contTravou2 = 0;
        }

        if (KartRigidbody.velocity.magnitude < 1.5f)
        {
            contTravou += Time.deltaTime / Time.timeScale;
            if (contTravou > 3)
                voltarNoCheckpoint();
        }
        else
        {
            contTravou = 0;
        }
    }

    private void Boost()
    {
        KartRigidbody.AddForce(transform.forward * 2500f, ForceMode.Impulse);
        if (!AplicandoEfeitoCam)
        {
            StartCoroutine(EfeitoCamera());
            AplicandoEfeitoCam = true;
        }
    }

    private void Lentidao()
    {
        if (lento && !imune)
        {
            KartRigidbody.mass = 1800;
            KartRigidbody.drag = 1;
            contLento += Time.deltaTime / Time.timeScale;
            if (contLento > 2)
                lento = false;
        }
        else
        {
            KartRigidbody.mass = 800;
            KartRigidbody.drag = 0.2f;
            contLento = 0;
        }
    }

    private void Imunidade()
    {
        if (imune)
        {
            PowerUpEspecialAyah.SetActive(true);
            contImunidade += Time.deltaTime / Time.timeScale;
            if (contImunidade > 20)
                imune = false;
        }
        else
        {
            PowerUpEspecialAyah.SetActive(false);
            contImunidade = 0;
        }
    }

    private void RastroPedras()
    {
        contRastro += Time.deltaTime / Time.timeScale;
        if (contRastro <= 5)
        {
            Instantiate(PowerUpEspecial, new Vector3 (PosTras.position.x + Random.Range(-0.3f,0.3f), 
                                                      PosTras.position.y, 
                                                      PosTras.position.z), PosTras.rotation); 
        }
        else
        {
            contRastro = 0;
            deixaRastro = false;
        }
    }

    private void ParticulasRastroFumaca()
    {
        if (Rastro != null)
        {
            if (KartRigidbody.velocity.magnitude < 1.5f)
            {
                if (Rastro.isPlaying)
                {
                    Rastro.Stop();
                }
            }
            else
            {
                if (!Rastro.isPlaying)
                {
                    Rastro.Play();
                }
            }
        }
    }

    private void foiAtingido()
    {
        if (!imune)
        {
            lento = true;
			if (Dano != null)
				Audio.PlayOneShot(Dano, VolumeDano);
			
            if (LevouDano != null)
                LevouDano.Play();
            else
            {
                if (!AplicandoEfeitoGlitch)
                {
                    AplicandoEfeitoGlitch = true;
                    StartCoroutine(EfeitoCameraGlitch());
                }
            }

        }
    }

    private IEnumerator EfeitoCameraGlitch()
    {
        EfeitoGlitch.enabled = true;
        yield return new WaitForSeconds(1f);
        EfeitoGlitch.enabled = false;
        AplicandoEfeitoGlitch = false;
    }

    private IEnumerator EfeitoAoColidir()
    {

		if (Dano != null)
			Audio.PlayOneShot(Dano, VolumeDano);
		
        if (LevouDano != null)
            LevouDano.Play();
        else
        {
            if (!AplicandoEfeitoGlitch)
            {
                AplicandoEfeitoGlitch = true;
                StartCoroutine(EfeitoCameraGlitch());
            }
        }
        yield return new WaitForSeconds(5f);
        FazendoEfeitoColidir = false;

    }

    private IEnumerator AnimacaoProvocacao()
    {
        Animacao.SetBool("Provocação", true);
        yield return new WaitForSeconds(TempoProvocacao);
        Animacao.SetBool("Provocação", false);
    }

    private void foiAtingidoMissel()
    {
        if (!imune)
        {
            lento = true;

            Audio.PlayOneShot(Explosao, VolumeExplosao);
            ExplosaoMissel.Play();

            if (Dano != null)
                Audio.PlayOneShot(Dano, VolumeDano);

            if (LevouDano != null)
                LevouDano.Play();
            else
            {
                if (!AplicandoEfeitoGlitch)
                {
                    AplicandoEfeitoGlitch = true;
                    StartCoroutine(EfeitoCameraGlitch());
                }
            }
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
                    Boost();
                    powerUpTipo = 0;
                }
                break;
            #endregion

            #region Missel Teleguiado
            case 3: //Missel Teleguiado
                {
                    ajustarDireçãoPowerUp(direção);
                    GameObject instancia = Instantiate(MisselGuiadoPrefab.gameObject, PowerUpPosition, PowerUpRotation) as GameObject;
                    instancia.GetComponent<MisselTeleguiadoScript>().defineAlvo(buscarAlvo(direção));
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
        StartCoroutine(AnimacaoProvocacao());
    }

    public void powerUpEspecial()
    {
        if (especialDisponivel)
        {
            switch (this.gameObject.name)
            {
                #region Violetta
                case "Kart_Violetta":
                    {
                        PowerUpVioletta();
                    }
                    break;
                #endregion
                #region Jeshi
                case "Kart_Jeshi":
                    {
                        PowerUpJeshi();
                    }
                    break;
                #endregion
                #region Momoto
                case "Kart_Momoto":
                    {
                        PowerUpMomoto();
                    }
                    break;
                #endregion
                #region Ayah
                case "Kart_Ayah":
                    {
                        PowerUpAyah();
                    }
                    break;
               #endregion
            }
            countEspecial = 0; //Inicia novamente a contagem do tempo de recarga
            StartCoroutine(AnimacaoProvocacao());
        }
    }

    #region Power Up Especial Violetta
    private void PowerUpVioletta()
    {
        Instantiate(PowerUpEspecial, PosFrente.position, PosFrente.rotation);
    }
    #endregion
    #region Power Up Especial Ayah
    private void PowerUpAyah()
    {
        imune = true;
    }
    #endregion
    #region Power Up Especial Jeshi
    private void PowerUpJeshi()
    {
        Instantiate(PowerUpEspecial, PosTras.position, PosTras.rotation);
    }
    #endregion
    #region Power Up Especial Momoto
    private void PowerUpMomoto()
    {
        deixaRastro = true;
    }
    #endregion

    #endregion

    #region Triggers e Colisoes

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Karts")
        {
            if (!FazendoEfeitoColidir)
            {
                StartCoroutine(EfeitoAoColidir());
                FazendoEfeitoColidir = true;
            }
        }
    }

    private void OnTriggerExit(Collider Objeto)
    {
        #region Aranha
        /*if (Objeto.gameObject.CompareTag("AranhaAoE"))
        {
            AranhaScript = Objeto.transform.parent.gameObject.GetComponent<AranhaExplosivaScript>();
            AranhaScript.removeAlvo = this.gameObject;
        }*/
        #endregion
    }

    private void OnTriggerEnter(Collider Objeto)
    {
        #region Area de Explosão da Aranha
        /*if (Objeto.gameObject.CompareTag("AranhaAoE"))
        {
            AranhaScript = Objeto.transform.parent.gameObject.GetComponent<AranhaExplosivaScript>();
            AranhaScript.addAlvo = this.gameObject;
        }*/
        #endregion
        #region Checkpoints
        if (Objeto.gameObject.CompareTag("Checkpoint"))
        {
            AuxContProg = Objeto.gameObject.GetComponent<CheckpointScript>().Num;
            if (ultimoCheckpoint == null)
            {
                contProgresso = AuxContProg;
                if (!Terminou)
                    ProgNoFim = contProgresso;
                ultimoCheckpoint = Objeto.gameObject;
                ContCP++;
            }
            else if (ultimoCheckpoint != Objeto.gameObject)
            {
                ultimoCheckpoint = Objeto.gameObject;
                contProgresso = AuxContProg;
                if (!Terminou)
                    ProgNoFim = contProgresso;
                ContCP++;
            }
        }
        #endregion
        #region Boost
        else if (Objeto.gameObject.CompareTag("Boost"))
        {
           Boost();
        }
        #endregion
        #region Randomização de PowerUps Comuns
        else if (Objeto.gameObject.CompareTag("PowerUpBox"))
        {
            if (powerUpTipo == 0)
            {
                powerUpTipo = Random.Range(1, 6);
                if (PegaPowerUp != null)
                    Audio.PlayOneShot(PegaPowerUp, VolumePegaPowerUp);
                PegouPowerUp.Play();

            }
        }
        #endregion
        #region Poça
        else if (Objeto.gameObject.CompareTag("Poca"))
        {
            Destroy(Objeto.gameObject);
            foiAtingido();
        }
        #endregion
        #region Missel
        else if (Objeto.gameObject.CompareTag("Missel"))
        {
            Destroy(Objeto.gameObject);
            foiAtingidoMissel();
        }
        #endregion
        #region Nuvem
        else if (Objeto.gameObject.CompareTag("Nuvem"))
        {
            Destroy(Objeto.gameObject);
            foiAtingido();
        }
        #endregion
        #region Contador de Voltas
        else if (Objeto.gameObject.CompareTag("LapCounter"))
        {
            if (!jaContou)
            {
                lap++;
                jaContou = true;
                StartCoroutine(DelayLapCounter());
            }
        }
        #endregion
        #region Limite de Pista
        else if (Objeto.gameObject.CompareTag("Limite"))
        {
            voltarNoCheckpoint();
        }
        #endregion   
    }

    #endregion

}
