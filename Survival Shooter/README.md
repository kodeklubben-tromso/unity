# Survival Shooter
Denne oppgaven tar utgangspunkt i [denne] (https://unity3d.com/learn/tutorials/projects/survival-shooter-project) Survival shooter på Unity sine sider.

Først må du laste ned et ferdig rammeverk for spillet [her] (https://www.assetstore.unity3d.com/en/#!/content/40756).
Inne på siden trykker du på "open in unity", da vil du komme i asset store i unity. Deretter finner du "Unity 5 Survival Shooter", trykker på den også på import for å laste inn prosjektet. 

## Omgivelser
- Velg `File -> New Scene` for å lage en ny scene, lagre den som Level 01 i Scenes mappen.
- Finn frem Evironment under prefabs og dra den til Hierarchy.
- Pass på at den er på Posisjon (0,0,0).
- Gjør det samme for Lights under prefab.
- Lag et nytt Gameobject (`Create -> 3D Object -> Quad`) og kall det Floor
- Sett Position til (0,0,0), Rotation til (90,0,0) og Scale til (100,100,0)
- Gå i Floor objektet og deaktiver "Mesh Renderer".
- Sett Floor objectet til å bruke Floor som layer.
- Lag et nytt tomt objekt (`GameObject -> Create Empty`).
- Kall det for BackgroundMusic.
- Klikk på `Add Component -> Audio -> Audio Source`.
- Der velger du bakgrunnsmusikken ved å trykke på sirkelen til høyre for Audio Clip så velger du Background Music.
- Huk av Loop under Audio Source og sett volumet til 0.1.

## Player karakteren
- Finn Player modellen i `Models -> Characters`, og dra den inn i Scenen.
- Sett posisjonen til (0,0,0) 
- Sett tag i Player til Player
- Høyreklikk på Animation mappen og trykk create etterfulgt av "Animation Controller" og gi den navnet `PlayerAC`
- Dra så den nye Animasjons kontrolleren inn i Player
- Dobbelt-klikk på PlayerAC da vil du endre fra bilde fra Scene til Animator
- Finn så frem attributtene til `Models -> Characters -> Player` dra så blokkene Death, Idle og Move inn i Animator vinduet
- Høyreklikk på Idle modusen og velg "Set as Default"
- Under Parameters klikk på pluss tegnet og velg "bool", kall denne for IsWalking.
- Lag så et Trigger parameter kalt "Die"
- Høreklikk så på Moduset idle og velg Make Transition, for så å trykke på Move
- Trykk på pila som du lagde mellom Idle og Move, under Inspection og Condition legg til IsWalking og sett den til True
- Høyreklikk så på Move og lag en overgang til Idle, Der skal du gjøre det samme men IsWalking skal være false
- Lag en overgang fra 'Any state' til Death
- Sett så condition til overgangen til Die
- Legg til Rigidbody til Player (`Player->Add Component->Physics->Rigidbody`), så setter du Drag og Angular Drag til "Infinity".
- Under Rigidbody og Constraints, huk av Freeze position Y og Freeze Rotation X og Y
- Legg til En Capsule Collider Component til Playeren (Sett Center til (0.2,0.6,0) og høyde til 1.2)
- Nå skal vi legge til lyd når spilleren blir skadet, legg til komponent Audio Source, Under Audio Clip legg til lyden "Player Hurt" og deaktiver "Play On Awake"
- Finn PlayerMovement scriptet under `Scripts->Player` og dra det på Playeren.
- Nå skal vi kode litt, åpne scriptet PlayerMovement i editoren.

```

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate ()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Move(h,v);
        Turning ();
        Animating(h,v);
    }

    void Move (float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
   } 

   void Turning ()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

       if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
       {
           Vector3 playerToMouse = floorHit.point - transform.position;
           playerToMouse.y = 0f;

           Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
           playerRigidbody.MoveRotation(newRotation);
       }

       
    }

    void Animating (float hideFlags, float v)
    {
        bool walking = hideFlags != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}

```

- Endre scriptet slik at det ser ut som det ovenfor
- Nå kan du prøve å lagre skripte å trykke Play!
- Når du skal stoppe det må du huske å stoppe play og ikke trykk på pause!

## Camera Opsett
- Velg Main Camera og sett posisjonen til (1, 15,-22) og rotasjon til (30,0,0) og endre Projection til `Orthographic`
- Sett Size under til 4.5 og sett Bakgrunnsfargen til svart
- Lag ett nytt script i Camera mappen som ligger inni Script mappen, kall det for `CameraFollow`
- Dra så CameraFollow scriptet over Main Camera, deretter åpne scriptet i en editor

```

using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float smoothing = 5f;

    Vector3 offset;

	void Start () {
        offset = transform.position - target.position;
	}
	
	void FixedUpdate () {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}

```

- Legg til Skriptet over i `CameraFollow`
- Lagre og gå tilbake til unity, her skal du dra Player objektet inn i 
Target under CameraFollow scriptet i Main Camera objektet
- For å lagre Player dra den inn i Prefabs mappen, lagre og trykk Play!

## Fiende nr 1

- Finn **Zombunny** modellen i **Models>Characters** mappen i prosjektet.
- Dra modellen inn på scenen
- Finn **HitParticles** i **Prefabs** mappen, dra den på **Zombunny** i **Hierarchy**
- Velg **Shootable** layeret for dette objektet.
- Legg til rigidbody til objektet ved å gå på **Add Component>Physics>Rigidbody**
- Sett **Drag** og **Angular Drag** til **Infinity**
- I **Constraints** frys posisjon **Y** og rotasjon **X** og **Y**
- Så **Add Component>Physics>Capsule Collider**
- Sett **Center Y** til **0.8** og **Height** til **1.5**
- Deretter **Add Component>Physics>Sphere Collider**
- Huk av **Is Trigger**
- Sett **Center Y** og **Radius**, begge til **0.8**
- **Add Component>Audio>Audio Source**
- Velg **Zombunny Hurt** lydklippet
- Slå av **Play On Awake** boksen
- **Add Component>Navigation>Nav Mesh Agent**
- Sett så **Radius** til **0.3**
- Sett **Speed** til **3**
- Sett **Stopping Distance** til **1.3**
- Sett **Height** til **1.1**
- Gå til **Window>Navigation**
- Velg **Bake** tabben øverst
- Sett **Radius** til **0.75**
- Sett **Height** til **1.2** og **Step Height** til **0.1**
- I **Advanced** området, sett **Width Inaccuracy %** til **1**
- Klikk **Bake** nederst.
- Velg **Animation** mappen i **Project** panelet.
- Høyreklikk den og **Create>Animation Controller**
- Navngi den **EnemyAC**, og dra den på **Zombunny** parent objektet i hierarkiet.
- Dobbelklikk **EnemyAC** for å åpne den i **Animator** vinduet.
- Expander **Zombunny** modellen i **Models>Characters** mappen i prosjekt panelet
- Det er 3 animasjoner der, **Idle, Move** og **Death**
- Dra hver av dem til Animatoren, begynn med **Move**
- Posisjoner **Idle** og **Move** nær hverandre, og put **Death** nær **Any State**
- Dobbelsjekk at **Move** er standard animasjonen/default, den burde være farget orange. Hvis ikke, så høyreklikk å velg **Set as Default**
- I **Animator** vinduet's **Parameters**, klikk på **+** og lag en **Trigger** parameter som heter **PlayerDead**
- Lag enda en **Trigger** parameter som heter **Dead**
- Høyreklikk på **Move** og lag en overgang/transition til **Idle**
- Høyreklikk på **Any State** og lag en overgang/transition til **Death**
- Sett **Condition** for **Move -> Idle** til **PlayerDead**
- Sett **Condition** for **Any State -> Death** til **Dead**
- Under **Scripts>Enemy** mappen, i prosjektet, finn **EnemyMovement** scriptet, og dra det på **Zombunny**
- Lagre scenen din.
- Dobbelklikk på script ikonet for å åpne det for redigering.
- Skriv inn koden under

```
using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform player;               // Reference to the player's position.
    PlayerHealth playerHealth;      // Reference to the player's health.
    EnemyHealth enemyHealth;        // Reference to this enemy's health.
    NavMeshAgent nav;               // Reference to the nav mesh agent.


    void Awake ()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent> ();
    }


    void Update ()
    {
        // If the enemy and the player have health left...
        if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            // ... set the destination of the nav mesh agent to the player.
            nav.SetDestination (player.position);
        }
        // Otherwise...
        else
        {
            // ... disable the nav mesh agent.
            nav.enabled = false;
        }
    } 
}

```

- Trykk på **Play** for å teste spillet.

## Health HUD

- Klikk på **2D** mode knappen.
- Velg **GameObject>UI>Canvas** fra menyen
- Gi nytt navn til ditt **Canvas**, det skal hete **HUDCanvas**
- **Add Component>Miscellaneous>Canvas Group**
- Slå av **Interactable** og **Blocks Raycasts** boksene.
- Høyreklikk **HUDCanvas>Create Empty**
- Kall den for **HealthUI**
- I **Rect Transform** klikk på **Anchor Presets**, og sett **HealthUI**'s **Anchor, Position** og **Pivot** til det nedre venstre hjørnet ved å holde inne **Alt-Shift** å så klikke på ankerpunktet.
- I **Rect Transform**, sett **Width** til **75** og **Height** til **60**
- Høyreklikk **HealthUI>UI>Image** 
- Kall Image for **Heart**
- I **Rect Transform** set **Position X og Y** til **0**
- Sett **Width** og **Height** til **30**
- I **Image** for **Source Image**, velg **Heart** spriten fra **Assets**
- Høyreklikk **HealthUI>UI>Slider**
- Kall **Slider** for **HealthSlider**
- I **Rect Transform**, set **Position X** til **95**, **Y** til **0**
- Ekspander **HealthSlider** for å vise children, velg **Handle Slide Area** childen til **HealthSlider** og slett den fra hierarkiet(delete).
- I **Slider** componenten til **HealthSlider**, sett **Transition** mode til **None**
- Set **Max Value** verdien til **100**, også sett **Value** til **100**.
- Høyreklikk **HUDCanvas** og lag et **UI>Image**
- Kall det for **DamageImage** og sett **Rect Transform** ankerpunkt presetten til **Stretch** i begge dimensjoner, ved å holde inne **Alt** mens du klikker den nederste høyre presetten.
- I **Image** komponenten, klikk på **Colour** blokken og set **Alpha**(A) verdien til **0**

## Player Health

- I **Scripts>Player** mappen, finn **PlayerHealth**, å dra denn inn på **Player** i hierarkiet.
- Åpne **PlayerHealth** scriptet å se på det.

```
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.


    Animator anim;                                              // Reference to the Animator component.
    AudioSource playerAudio;                                    // Reference to the AudioSource component.
    PlayerMovement playerMovement;                              // Reference to the player's movement.
    PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
    bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged.


    void Awake ()
    {
        // Setting up the references.
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();

        // Set the initial health of the player.
        currentHealth = startingHealth;
    }


    void Update ()
    {
        // If the player has just been damaged...
        if(damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        damaged = false;
    }


    public void TakeDamage (int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // Play the hurt sound effect.
        playerAudio.Play ();

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if(currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death ();
        }
    }


    void Death ()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

        // Turn off any remaining shooting effects.
        playerShooting.DisableEffects ();

        // Tell the animator that the player is dead.
        anim.SetTrigger ("Die");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        playerAudio.clip = deathClip;
        playerAudio.Play ();

        // Turn off the movement and shooting scripts.
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }       
}

```

- Legg skriptet over til i script filen
- Gå så tilbake til Unity å i **PlayerHealth (Script)** komponenten, legg til **HealthSlider** fra hierarkiet til **Health Slider** public variable, ved å dra det dit.
- På samme komponenten legg til **DamageImage** fra hierarkiet til **Damage Image** public variable, ved å dra det dit.
- På **PlayerHealth (Script)** komponenten, legg til **Player Death** lydklippet til **Death Clip**.
- Finn **EnemyAttack** i **Scripts>Enemy** mappen til prosjektet, og dra den på **Zombunny** i hierarkiet.
- Åpne **EnemyAttack** scriptet for redigering ved å dobbelklikke på script ikonet i prosjektet.

```
using UnityEngine;
using System.Collections;


public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
    public int attackDamage = 10;               // The amount of health taken away per attack.


    Animator anim;                              // Reference to the animator component.
    GameObject player;                          // Reference to the player GameObject.
    PlayerHealth playerHealth;                  // Reference to the player's health.
    EnemyHealth enemyHealth;                    // Reference to this enemy's health.
    bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
    float timer;                                // Timer for counting up to the next attack.


    void Awake ()
    {
        // Setting up the references.
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }


    void OnTriggerEnter (Collider other)
    {
        // If the entering collider is the player...
        if(other.gameObject == player)
        {
            // ... the player is in range.
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        // If the exiting collider is the player...
        if(other.gameObject == player)
        {
            // ... the player is no longer in range.
            playerInRange = false;
        }
    }


    void Update ()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            // ... attack.
            Attack ();
        }

        // If the player has zero or less health...
        if(playerHealth.currentHealth <= 0)
        {
            // ... tell the animator the player is dead.
            anim.SetTrigger ("PlayerDead");
        }
    }


    void Attack ()
    {
        // Reset the timer.
        timer = 0f;

        // If the player has health to lose...
        if(playerHealth.currentHealth > 0)
        {
            // ... damage the player.
            playerHealth.TakeDamage (attackDamage);
        }
    }
}

```

- Legg til scriptet ovenfor i scriptfilen.
- Gå så tilbake til unity å lagre prosjektet ditt.

## [Harming enemies](http://unity3d.com/learn/tutorials/projects/survival-shooter/harming-enemies?playlist=17144) 

Nå skal vi gjøre det mulig å skade fiendene! 

- I `Scripts -> Enemy`-mappa finner du `EnemyHealth` scriptet. Dra og slipp det
  på **Zombunny** i hierarkiet 
- I `Enemy Health` script componenten gir du `Zombunny death`-lydklippet til
  `Death Clip` variabelen: 
- Åpne `EnemyHealth` scriptet og ta en titt på variablene. Her finner dere
  variabler for hvor mye liv fienden har, om den er død eller levende, om den
  synker gjennom gulvet osv. Om du vil ha en god gjennomgang av scriptet kan du
  se [denne](https://youtu.be/l86gpYbQFzY?t=1m). 

Nå skal vi få til litt skyting! 

- I `Project > Prefabs` trykk på GunParticles
- Klikk på tannhjulet til høyre og velg `Copy Component`
- Utvid **Player** game objektet i hierarkiet og velg **GunBarrelEnd**
- Klikk på tannhjulet og velg **Paste Component as New**
- Lukk den nye **Particle System** componenten og med **GunBarrelEnd** fortsatt
  valgt legger du til en **Line Renderer**-komponent: `Add Component > Effects >
  Line Renderer`. 
- Utvid **Materials**-området og bruk sirkelen til å velge **LineRenderMaterial**. 
- Utvid parametrene til **Line Renderer** og sett **Line Renderer** sin **Start
  Width** og **End Width** til **0.05**. 
- Skru av **Line Renderer** komponenten ved å trkke bort krysset i boksen

Nå trenger vi å lage litt lys som lyser opp når spilleren skyter 

- La oss lage et lys, `Add Component > Rendering > Light`.
- Velg en gulfarge gra **Color blick / Picker**
- Skru av **Light**-komponenten ved å krysse den ut
- Legg til en ny **Audio Source** komponent: `Add Component > Audio > Audio
  Source` og velg **Player Gunshot** som lydklipp
- Klikk bort **Play On Awake** for denne lydkilden 
- I `Project > Scripts > Player`-mappa gi **PlayerShooting** til
  **GunBarrelEnd** i hierarkiet
- Velg **Player** i hierarkiet og klikk på **Apply** på toppen av Inspector-en
  for å oppdatere **Prefab**-en vår. 

La oss teste at alt fungerer bra! 

- Lagre scenen og trykk Play! 
- Om du får noen feil åpne opp **EnemyMovement**-scriptet i `Scripts > Enemy`
    og ta bort alle `//` i scriptet og lagre scriptet 
- Det kan også være noe i **PlayerHealth** så åpne det også og fjern alle `//`.


## [Scoring Points](http://unity3d.com/learn/tutorials/projects/survival-shooter/scoring-points?playlist=17144)
Nå skal vi gi spilleren poeng for å skyte zombunnies og oppdatere denne for hver
gang han skyter en! 

- Velg **HUDCanvas** i hierarkiet og høyreklikk for å lage et nytt **Tekst**
objekt. `UI > Text`. Husk at den må ligge i **HUDCanvas** i
Hierarkiet. 
- Kall dette objektet **ScoreTet**
- Sett **Anchor position** i **RectTransform** til **Top Center**. 
- Sett `x` posisjon til **0** og `y` posisjon til **-55**.
- Endre `width` til **300** og `height` til **50**
- I **Text** komponenten setter du teksten til **"Score: "**. Denne vil vi endre
  på etterhvert til å vise hvor mange poeng spilleren har
- Sett **Font size** til **50** for litt større skrift
- Sett **Alignments** til **Center** og **Middle**
- Endre skriftfargen til hvit ved å klikke på **color**-blokka 

Nå skal vi legge litt skygge til teksten.

- Trykk på **Add Component**, søk etter **Shadow** og legg den til. Velg
  **Effect Distance** til `(2, -2)`. 
- I `Scripts > Managers` finn fram **ScoreManager**-scriptet og dra den over
  **ScoreText**-spillobjektet. 

Nå skal vi oppdatere **EnemyHealth**-scriptet

- Velg **Zombunny** i hierarkie, finn **EnemyHealth** scirptet og åpne det. 
- Fjern `//` på linje 77
- Lagre den og trykk **Play** i unity for å teste prosjektet! 

For å få flere fiende lagrer vi **Zombunny** som en prefab: 

- Dra **Zombunny** inn i **Prefabs**-mappa
- Fjern **Zombunny** fra hierarkiet og lagre scenen


## [More Enemies](http://unity3d.com/learn/tutorials/projects/survival-shooter/more-enemies?playlist=17144)
La oss lage en **Zombear**! 

- I prefabs-mappa velger du **ZomBear**
- Åpne **Zombear** sin **Animator** og fra prosjektet drar du inn **EnemyAC**
  fra **Animation**-mappa. Denne skal dras over til **Animator controller**

Nå kommer en **Hellephant**

- I **prefabs**-mappa velger du **Hellephant**, 
- Velg **Animation**-mappa og lag en **Animator Override Controller**: `Create >
  Animator Override Controller`.
- Kall den **HellephanAOC**
- Gi **EnemyAC** til **Controller** egenskapen til høyre i **inspector**-en


- I `Models > Characters` utvid **Hellephant** for å se på animasjonene
- Dra **Idle**, **Move**, og **Death** til de ledige feltene i **HellephanAOC
  Override**-tabellen
- Velg **Hellephant** i prefabs-mappa og gi **HellephanAOC** til dens **Animator
  Controller**. 

La oss få styr på de nye fiendene våre 
- Lag et tomt objekt: `GameObject > Create Empty` og kall den **EnemyManager**
- I `Script > Managers` finner du **EnemyManager** scriptet og drar den over til
  **EnemyManager**

La oss lage noen spawn points to zombiene våre 
- Lag et tomt objekt: `GameObject > Create Empty` og kall den
  **ZombunnySpawnPoint**
- I inspectoren setter du **Gizmo**-en til **ZombunnySpawnPoint** til en fin
  blåfarge.
- Sett posisjonnen til `(-20.5, 0, 12.5)` 
- Sett rotasjon til `(0,130,0)`

La oss lage spawn points to zombears også

- Lag et tomt objekt: `GameObject > Create Empty` og kall den
  **ZombearSpawnPoint**
- Sett **Gizmo**-en til **ZombearSpawnPoint** rosa
- Sett posisjonen til `(22.5,0,15)`
- Sett rotasjon til `(0,240,0)`


La oss lage spawn points to hellephants også

- Lag et tomt objekt: `GameObject > Create Empty` og kall den
  **HellephantSpawnPoint**
- Sett **Gizmo**-en til **HellephantSpawnPoint** gul
- Sett posisjonen til `(0,0,32)`
- Sett rotasjon til `(0,230,0)`

La oss spawne noen fiender

- Velg **EnemyManager** i hierarkiet. I **EnemyManager** komponenten setter du
  **Player** til **PlayerHealth** variablen
- I prefabs-mappa drar du **Zombunny** inn som **Enemy** 
- Dobbelsjekk at **Spawn Time** er satt til 3 sekunder, hvis ikke blir det
  kanskje litt for mange fiender! 
- Dra inn **ZombunnySpawnPoint** fra hierarkiet til tittelen til
  **SpawnPoints**-arrayet 
- Lagre og test spillet! 

Nå må vi få inn de andre fiendene våre også 
- I `Scripts > Managers`-mappa finner du **EnemyManager** scriptet og drar det
  inn i **EnemyManager** objektet 2 ganger til
- Sjekk at det er 3 **EnemyManager** spawner-komponenter i **EnemyManager**
  objektet
- Dra over **Player** objektet til **PlayerHealth** variablene til begge de nye
  **EnemyManager** komponentene. 
- Fra **Prefabs**-mappa drar du **Zombear** til **Enemy**-feltet til den andre **EnemyManager** 
- Fra **Prefabs**-mappa drar du **Hellephant** til **Enemy**-feltet til den tredje **EnemyManager** 
- Dra inn **ZombearSpawnPoint** fra hierarkiet til tittelen til **SpawnPoints**
  arrayet til den andre **EnemyManager**
- Dra inn **HellephantSpawnPoint** fra hierarkiet til tittelen til **SpawnPoints**
  arrayet til den tredje **EnemyManager**
- I den tredje **EnemyManager** til **Hellephant**-en setter du **Spawn Time**
  til 10.
- Lagre og test spillet! 

## [Game Over](http://unity3d.com/learn/tutorials/projects/survival-shooter/game-over?playlist=17144)
La oss legge inn et bilde som vises når spillet er over


- Høyreklikk på **HUDCanvas** og lag en **UI > Image**
- Kall denne **ScreenFader**
- I **Rect Transform** klikk på **Anchor Presets** og **Alt-klikk** på **Stratch
  both**.
:make
- I **Image** komponenten klikker du på **Color** og velger en lys blåfarge

- Høyreklikk på **HUDCanvas** og lag en **UI > Text**
- Kall denne **GameOverText**
- I **Rect Transform** klikker du på **Anchor Presets** og **Alt-klikk** på
  **Middle center**
- Sett `width` til **300** og `height` til **50**

- I **Text**-komponenten setter du teksten til å være **'Game Over!'**
- Bytt **skrifttype** til **Luckiest Guy**
- Sett **skriftstørrelse** til **50** og **Alignment** til **Middle** og **Center**
- Sett **sktiftfarge** til hvit 
- Legg til en **Shadow** komponent: **Add component** og søk etter **Shadow**. 

- Stokk om i hierarkiet slik at det som ligger under HUDCanvas ser slik ut: 

```
HUDCanvas
- HealthUI
- DamageImage
- ScreenFader
- GameOverText
- ScoreText
```

- Velg **ScreenFader** fra i hierarkiet
- I **Image** komponenten setter du **Color** sin **alpha**-verdi til 0
- Velg **GameOverText** i hierarkiet og sett **Color** sin **alpha**-verdi til 0



- Velg **HUDCanvas** i hierarkiet
- Gå til **Window > Animation** og klikk på **Add Curve**
- I **Create Animation** vinduet velger du **Animation**-mappa som destinasjon
  og kall den **GameOverClip**. (Unity lager en **Animator Controller** også,
  men den trenger du ikke bry deg om nå)

- Lag **Curve** til **GameOverText > Text > Color**
- Lag **Curve** til **GameOverText > RectTransform > Scale** 
- Lag **Curve** til **ScreenFader > Image > Color**
- Lag **Curve** til **ScoreText > RectTransform > Scale** 
- Velg og flytt alle **end keyframes** til **0:30** 


- Flytt markøren i tidslinja til **0:20**, velg **GameOverText > RectTransform >
  Scale** og trykk **K** for å lage en **keyframe**. 
- Gå til frame **0** og velg **GameOverText > RectTransform > Scale** og sett
  verdiene til **0**. 
- Flytt markøren i tidslinja til **0:20** og sett **GameOverText > RectTransform
  > Scale** verdiene til **1.2**

- Flytt markøren til **0:30** og sett 
    - **GameOverText > Text > Color > Alpha** til **1**
    - **ScreenFader > Image > Color > Alpha** til **1**
    - **Score Text > RectTransform > Scale ** til **0.8**
- Velg alle **Keyframes** og flytt dem slik at de starter på **1:30** (frame 90)
  og disable **Record** mode 



- I **Project** panelet åpner du **Animation** mappa og velger **GameOverClip**,
  klikk bort **Loop time**
- Velg nå **HUDCanvas** og dobbel klikk på den for å åpne den i
  **Animator**-vinduet
- I **Animator**-vinduet høyreklikker du og velger **Create State > Empty** og
  kall den **Empty**
- Høyreklikk på **Empty state** og **Create Transition** til
  **GameoverClip**-staten ved å velge den
- Lag en ny **Animator Trigger**-parameter **GameOver**



- Høyreklikk på **Empty**-staten og velg **Set as Default**
- Velg overgangen fra **Empty** til **GameOverClip** og sett **Condition** til
  **GameOver** 
- Velg **HUDCanvas** fra hierarkiet og dra inn **GameOverManager** fra **Scripts
  > Managers** mappa til **HUDCanvas**. 

- Dra inn **Player** fra hierarkiet til **Player Health** variabelen til
  **GameOverManager** script-komponenten
- **File > Save Scene** og **File > Save Project**

## SPILL SPILLET DITT! 

### Bonus: Lek rundt med forskjellige variabler for å endre på spawn tidere og hastighet. 

