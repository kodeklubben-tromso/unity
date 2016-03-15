# Tanks
Denne oppgaven tar utgangspunkt i [dette](http://unity3d.com/learn/tutorials/projects/tanks-tutorial) prosjektet på Unity sine sider.

Først så må vi starte Unity og velge et nytt prosjekt. Prosjektet kan man kalle hva man selv vil, vi kommer til å kalle det for Tanks i denne tutorialen. Når vi har åpnet Unity så må vi laste ned alle prosjekt filene, dette gjør vi ved å velge **Window** oppe på verktøylinjen og velger **Asset Store**. Inne i **Asset Store** så søker vi på **Tanks Tutorial**. Når vi har funnet prosjektet så klikker vi på Download og Import etter nedlastingen er ferdig.


## 1 Sette Opp Prosjektet

- Velg `File -> New Scene` oppe på verktøylinjen for å lage en ny Scene, lagre den nye scenen som `Main` i `Scenes` mappen.
- Slett lyskilden som heter `Directional Light` i hierarkiet fra scenen.
- Dra `Prefaben` som heter **Level Art** inn i hierarkiet. Denne ligger i mappen `Prefabs` i prosjektet:

![ScreenShot](Pictures/levelart.png)

- Fra `Window` menyen, åpne `Lighting` panelet. På dette panelet så skrur vi av **Auto** helt nederst på vinduet og **Baked GI**. Under **Precomputed Realtime GI** så setter vi `Realtime Resolution` til 0.5:

![ScreenShot](Pictures/bakedgi.png)

- Under **Environment Lighting** forandre `Ambient Source` fra `Skybox` til `Color` og bytt fargen til RGB verdiene (72,62,113).
- Klikk Build nederst på Panelet.
- Lukk `Lighting` panelet, eller klikk på `Inspector` panelet.
- Velg `Main Camera` fra hierarkiet og bytt posisjon til (-43,42,-25)
- Bytt rotasjon til (40,60,0)
- Forandre kameraet sin `Projection` til `Ortographic`
- Forandre kameraet sin `Clear Flags` fra `Skybox` til `Solid Color`.
- Forandre `Background` farge til RGB (80,60,50)
- Lagre Scenen din.

![ScreenShot](Pictures/camerasetup.png)

# Miniquiz

### Hva kalles de tre koordinatene vi bruker til å si posisjonen til noe i Unity?
1. ZXE
2. BHD
3. XYZ
4. ABC

### Hvilke 3 farger består RGB av?
1. Rød, Gul, Brun
2. Rosa, Grønn, Beige
3. Rosa, Grønn, Burgunder
4. Rød, Grønn, Blå




## 2 Lage En Tank

- Under mappen `Models` finner du en modell som heter `Tank`. Dra denne inn i hierarkiet, på samme måte som vi gjorde med `LevelArt`.
- Forandre `Layer` til tanksen slik at den ligger på `Players` layeret. I Dialogboksen velger du `No, for this object only`

![ScreenShot](Pictures/tanklayer.png)

- Legg til en `Rigidbody` til tanksen.
- Under `Constraints` inne i `Rigidbody`, huk av for `Freeze Position` på Y-aksen, og huk av for `Freeze Rotation` på X-aksen og Z-aksen.
- Legg til en `Box Collider` på tanksen og forandre `Center` til (0, 0.85, 0) og forandre størrelsen til (1.5, 1.7, 1.6).
- Legg til en `Audio Source` til tanksen vår og forandre `Audio Clip` til `Engine Idle`. Huk også av for `Loop`.

![ScreenShot](Pictures/audiosource.png)

- Legg til en ekstra `Audio Source` på tanksen vår og slå av `Play On Awake`.
- Velg `Prefabs` mappen i prosjektet vårt og dra tanksen ned til mappen, det som skjer er at vi lager en `Prefab` av tanksen vi har lagd til nå. En `Prefab` er et ferdiglaget objekt som inneholder alt vi har lagt til tanksen vår til nå.

![ScreenShot](Pictures/tankprefab.png)

- Fra `Prefabs` mappen, dra `DustTrail` prefaben til tanksen vår i hierarkiet, slik at den blir en `Child` av tanks objektet vårt.
- Dupliser `DustTrail` ved å bruke Ctrl + D slik at vi får to `DustTrail` på tanksen vår. Gi den nye `DustTrail` navnet `LeftDustTrail` og den andre navnet `RightDustTrail`.
- Sett posisjonen til `LeftDustTrail` til (-0.5, 0, -0.75) og posisjonen til `RightDustTrail` til (0.5, 0, -0.75)

### Tid for litt Kode
- I `Scripts/Tank` folderen så finner vi et script som heter `TankMovement`, dra denne over på tanksen vår i hierarkiet.
- Dobbelklikk på `TankMovement` scriptet for å åpne scriptet i Visual Studio eller MonoDevelop.
- I scriptet så har vi noen ferdiglagde funksjoner, disse er kommentert ut, så vi må fjerne kommentar tegnene `/* */`
- Koden for scriptet ligger under, men vi vil gjerne tenke oss frem til hva vi vil gjøre først.


```

 	private void Update ()
    {
        // Store the value of both input axes.
        m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis (m_TurnAxisName);

        EngineAudio ();
    }


    private void EngineAudio ()
    {
        // If there is no input (the tank is stationary)...
        if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                // ... change the clip to idling and play it.
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play ();
            }
        }
        else
        {
            // Otherwise if the tank is moving and if the idling clip is currently playing...
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // ... change the clip to driving and play.
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }


    private void FixedUpdate ()
    {
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        Move ();
        Turn ();
    }


    private void Move ()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn ()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
    }
```

# Miniquiz

### Hva er en Prefab?
1. En Unity pakke for Grafikk
2. Et ferdiglagd objekt som vi kan bruke i Unity
3. Et bilde
4. En ny smarttelefon

### Hva er en variabel?
1. Et navn som holder en verdi i programmet vårt
2. Et program som varierer i hvor bra det er
3. En ukjent verdi som vi ikke kan se på
4. Et program som gir varierende resultat


### Hva betyr public i Unity?
1. Public betyr at noe kan nåes overalt i programmet vårt
2. Public betyr at det ligger ute på nett
3. Public betyr at noe er lagd i all offentlighet
4. Public betyr ingenting i Unity


## 3 Kamera Kontroll

- Lag et nytt `Empty GameObject` i hierarkiet og kall det for `Camera Rig`

![ScreenShot](Pictures/createempty.png)

- Sett posisjonen til `Camera Rig` tilbake til (0, 0, 0) og sett `Rotation` til (40, 60, 0)
- I hierarkiet, dra `Main Camera` til det nye `Camera Rig` objektet vårt, slik at `Main Camera` blir et child av `Camera Rig`
- Sett posisjonen til `Main Camera` (Ikke `Camera Rig`) til (0, 0, -65)
- Lag et nytt script under `Scripts` som heter `CameraControl` og åpne scriptet.
- I dette scriptet så skal vi sette opp kameraet til å Zoome ved å sette størrelsen på the ortografiske kameraet.
- Vi vil også at kameraet skal følge begge tanksene.

```

	using UnityEngine;
	
	public class CameraControl : MonoBehaviour
	{
	    public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
	    public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
	    public float m_MinSize = 6.5f;                  // The smallest orthographic size the camera can be.
	    [HideInInspector] public Transform[] m_Targets; // All the targets the camera needs to encompass.
	
	
	    private Camera m_Camera;                        // Used for referencing the camera.
	    private float m_ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
	    private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
	    private Vector3 m_DesiredPosition;              // The position the camera is moving towards.
	
	
	    private void Awake ()
	    {
	        m_Camera = GetComponentInChildren<Camera> ();
	    }
	
	
	    private void FixedUpdate ()
	    {
	        // Move the camera towards a desired position.
	        Move ();
	
	        // Change the size of the camera based.
	        Zoom ();
	    }
	
	
	    private void Move ()
	    {
	        // Find the average position of the targets.
	        FindAveragePosition ();
	
	        // Smoothly transition to that position.
	        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
	    }
	
	
	    private void FindAveragePosition ()
	    {
	        Vector3 averagePos = new Vector3 ();
	        int numTargets = 0;
	
	        // Go through all the targets and add their positions together.
	        for (int i = 0; i < m_Targets.Length; i++)
	        {
	            // If the target isn't active, go on to the next one.
	            if (!m_Targets[i].gameObject.activeSelf)
	                continue;
	
	            // Add to the average and increment the number of targets in the average.
	            averagePos += m_Targets[i].position;
	            numTargets++;
	        }
	
	        // If there are targets divide the sum of the positions by the number of them to find the average.
	        if (numTargets > 0)
	            averagePos /= numTargets;
	
	        // Keep the same y value.
	        averagePos.y = transform.position.y;
	
	        // The desired position is the average position;
	        m_DesiredPosition = averagePos;
	    }
	
	
	    private void Zoom ()
	    {
	        // Find the required size based on the desired position and smoothly transition to that size.
	        float requiredSize = FindRequiredSize();
	        m_Camera.orthographicSize = Mathf.SmoothDamp (m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
	    }
	
	
	    private float FindRequiredSize ()
	    {
	        // Find the position the camera rig is moving towards in its local space.
	        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);
	
	        // Start the camera's size calculation at zero.
	        float size = 0f;
	
	        // Go through all the targets...
	        for (int i = 0; i < m_Targets.Length; i++)
	        {
	            // ... and if they aren't active continue on to the next target.
	            if (!m_Targets[i].gameObject.activeSelf)
	                continue;
	
	            // Otherwise, find the position of the target in the camera's local space.
	            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);
	
	            // Find the position of the target from the desired position of the camera's local space.
	            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;
	
	            // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
	            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
	
	            // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
	            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
	        }
	
	        // Add the edge buffer to the size.
	        size += m_ScreenEdgeBuffer;
	
	        // Make sure the camera's size isn't below the minimum.
	        size = Mathf.Max (size, m_MinSize);
	
	        return size;
	    }
	
	
	    public void SetStartPositionAndSize ()
	    {
	        // Find the desired position.
	        FindAveragePosition ();
	
	        // Set the camera's position to the desired position without damping.
	        transform.position = m_DesiredPosition;
	
	        // Find and set the required size of the camera.
	        m_Camera.orthographicSize = FindRequiredSize ();
	    }
	}


```

- Dra `Camera Control` scriptet over på vår `Camera Rig`
- Test spillet
- Lager Scenen

# Miniquiz

### For en skjerm som har 1920x1080 pixler og `Aspect Ratio` 16:9, hvordan finner vi aspectet?
1. 1920 / 1080
2. 1920 * 1080
3. 1080 * 9
4. 16/9

### Med et `Ortografisk` kamera, hva skjer når vi zoomer inn?
1. Vi flytter kameraets posisjon nærmere
2. Vi øker kameraets ortografiske størrelse
3. Vi minsker kameraets ortografiske størrelse
4. Øker kameraets aspekt


### Hva betyr `Aspect Ratio`?
1. Størrelsen til hvert pixel
2. Bredden på skjermen
3. Forholdet mellom størrelser i x og y retning
4. Høyden på skjermen



## 4 Helse
- Sett transform toggelen til `Pivot`, ikke `Center`

![ScreenShot](Pictures/pivot.png)

- Lag en `Slider` som ligger under `GameObject > UI > Slider`

![ScreenShot](Pictures/slider.png)

- Når man lager en `Slider` så kan man se at det dukker opp flere komponenter slik som `Canvas` og `EventSystem`, disse må være her for at Helsen til tanksen vår skal fungere korrekt.
- Velg nå `EventSystem` objektet i hierarkiet og under `Standalone Input Model` komponenten, forandre `Horizontal Axis` og `Vertical Axis` til `HorizontalUI` og `VerticalUI`.

![ScreenShot](Pictures/standalone.png)

- Velg `Canvas` objektet og forandre `Reference Pixels per Unit` til 1 under `Canvas Scaler` komponenten. Forandre så `Render Mode` til `World Space` under `Canvas` komponenten til `Canvas` objektet.
- I hierarkiet, dra `Canvas` til `Tank` objektet for å gjøre `Canvas` til et child av `Tanksen`.
- Sett posisjonen til `Canvas` til (0, 0.1, 0).
- Forandre `Width` og `Height` til 3.5
- Forandre rotasjonen til (90, 0, 0)
- Lagre scenen

- Klikk den lille pilen ved siden av `Canvas` så man kan se alt den inneholder.
- Velg `HandleSlideArea` og slett denne.
- Velg så alle elementene som ligger under `Canvas` og klikk på `Anchor Preset` knappen

![ScreenShot](Pictures/select.png)

- Hold inne `Alt` knappen på tastaturet og klikk på den preseten som ligger i høyre hjørne

![ScreenShot](Pictures/anchor.png)

- Du vil nå se at den lille firkanten som ligger under tanksen er grå
- Klikk så på `Slider` under `Canvas` og slå av knappen `Interactable`.
- Forandre `Transition` til None.
- Sett verdien til slideren til 100.
- Skift navn på slideren til `HealthSlider` og velg `Background` objektet på slideren.
- På `Source Image` komponenten så forandrer vi `Background` til `Health Wheel`
- Trykk på `Color` og forandre `A` (eller `Alpha`) verdien til 80
- Klikk på `Fill` child objektet, og under `Source` image velg `Health Wheel` igjen.
- Skift `A` til 150 under `Color`

- Velg så `Image Type` filled. `Fill Origin` skal stå på left og slå av knappen `Clockwise`

![ScreenShot](Pictures/fill.png)

- I `Scripts` folderen, dra scriptet `UIDirectionControl` til `HealthSlider`
- Klikk på Tanks objektet i hierarkiet og klikk `Apply` helt på toppen for å oppdatere prefaben.
- Lagre Scenen din.

- Åpne `Prefabs` folderen og dra `TankExplosion` inn i hierarkiet.
- Velg `TankExplosion` og legg til en `Audio Source` i komponentene. Under `Audio Clip` velg `TankExplosion` lyden og klikk av `Play On Awake`.
- Klikk `Apply` på `TankExplosion` objektet. Og så slett objektet når du er ferdig.
- Finn `TankHealth` scriptet i `Scripts/Tank` og dra det til `Tank` objektet, åpne så scriptet.

```

	using UnityEngine;
	using UnityEngine.UI;
	
	public class TankHealth : MonoBehaviour
	{
	    public float m_StartingHealth = 100f;               // The amount of health each tank starts with.
	    public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
	    public Image m_FillImage;                           // The image component of the slider.
	    public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
	    public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
	    public GameObject m_ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.
	    
	    
	    private AudioSource m_ExplosionAudio;               // The audio source to play when the tank explodes.
	    private ParticleSystem m_ExplosionParticles;        // The particle system the will play when the tank is destroyed.
	    private float m_CurrentHealth;                      // How much health the tank currently has.
	    private bool m_Dead;                                // Has the tank been reduced beyond zero health yet?
	
	
	    private void Awake ()
	    {
	        // Instantiate the explosion prefab and get a reference to the particle system on it.
	        m_ExplosionParticles = Instantiate (m_ExplosionPrefab).GetComponent<ParticleSystem> ();
	
	        // Get a reference to the audio source on the instantiated prefab.
	        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource> ();
	
	        // Disable the prefab so it can be activated when it's required.
	        m_ExplosionParticles.gameObject.SetActive (false);
	    }
	
	
	    private void OnEnable()
	    {
	        // When the tank is enabled, reset the tank's health and whether or not it's dead.
	        m_CurrentHealth = m_StartingHealth;
	        m_Dead = false;
	
	        // Update the health slider's value and color.
	        SetHealthUI();
	    }
	
	
	    public void TakeDamage (float amount)
	    {
	        // Reduce current health by the amount of damage done.
	        m_CurrentHealth -= amount;
	
	        // Change the UI elements appropriately.
	        SetHealthUI ();
	
	        // If the current health is at or below zero and it has not yet been registered, call OnDeath.
	        if (m_CurrentHealth <= 0f && !m_Dead)
	        {
	            OnDeath ();
	        }
	    }
	
	
	    private void SetHealthUI ()
	    {
	        // Set the slider's value appropriately.
	        m_Slider.value = m_CurrentHealth;
	
	        // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
	        m_FillImage.color = Color.Lerp (m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
	    }
	
	
	    private void OnDeath ()
	    {
	        // Set the flag so that this function is only called once.
	        m_Dead = true;
	
	        // Move the instantiated explosion prefab to the tank's position and turn it on.
	        m_ExplosionParticles.transform.position = transform.position;
	        m_ExplosionParticles.gameObject.SetActive (true);
	
	        // Play the particle system of the tank exploding.
	        m_ExplosionParticles.Play ();
	
	        // Play the tank explosion sound effect.
	        m_ExplosionAudio.Play();
	
	        // Turn the tank off.
	        gameObject.SetActive (false);
	    }
	}

```

- Lagre koden din, og trykk `Apply` på toppen av `Tank` objektet.
- Lagre Scenen.
##
- Nå må vi sette opp alle `public` variabler som vi skal bruke i `TankHealth` scriptet vårt.
- Velg `Tank` objektet i hierarkiet
- Dra `HealthSlider` til `Slider` variablen vår på `TankHealth` scriptet, dette vil ligge nederst på Tank objektet i `Inspector`
- Gjør det samme med `Fill` objektet på Tanksen vår, den skal draes til `FillImage`
- Dra `TankExplosion` prefaben til `ExplosionPrefab` variablen
- Klikk `Apply` på tanksen vår og lagre Scenen

## 5 Kuler
- Finn `Shell` i `Models` mappen og dra denne inn i hierarkiet
- Legg til en `Capsule Collider` på `Shell` objektet
- Slå på `IsTrigger` under `Capsule Collider` som vi akkurat la til
- Sett `Direction` på collideren til Z-aksen
- Forandre `Center` av `Capsule Collider` til (0, 0, 0.2)
- Forandre `Radius` til 0.15 og `Height` til 0.55
- Legg til en `Rigidbody` på `Shell` objektet
##
- Finn prefaben `ShellExplosion` i `Prefabs` mappen og dra den til `Shell` slik at den blir en child av `Shell`
- Legg til en `AudioSource` til `ShellExplosion`
- Sett `AudioClip` til `ShellExplosion` og slå av `Play on Awake`
##
- Velg `Shell` objektet igjen og legg til en `Light` komponent
- I `Scripts/Shell` folderen, dra `ShellExplosion` scriptet til `Shell`
- Åpne scriptet

```

	using UnityEngine;
	
	public class ShellExplosion : MonoBehaviour
	{
	    public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
	    public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
	    public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
	    public float m_MaxDamage = 100f;                    // The amount of damage done if the explosion is centred on a tank.
	    public float m_ExplosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
	    public float m_MaxLifeTime = 2f;                    // The time in seconds before the shell is removed.
	    public float m_ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.
	
	
	    private void Start ()
	    {
	        // If it isn't destroyed by then, destroy the shell after it's lifetime.
	        Destroy (gameObject, m_MaxLifeTime);
	    }
	
	
	    private void OnTriggerEnter (Collider other)
	    {
	        // Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
	        Collider[] colliders = Physics.OverlapSphere (transform.position, m_ExplosionRadius, m_TankMask);
	
	        // Go through all the colliders...
	        for (int i = 0; i < colliders.Length; i++)
	        {
	            // ... and find their rigidbody.
	            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody> ();
	
	            // If they don't have a rigidbody, go on to the next collider.
	            if (!targetRigidbody)
	                continue;
	
	            // Add an explosion force.
	            targetRigidbody.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);
	
	            // Find the TankHealth script associated with the rigidbody.
	            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth> ();
	
	            // If there is no TankHealth script attached to the gameobject, go on to the next collider.
	            if (!targetHealth)
	                continue;
	
	            // Calculate the amount of damage the target should take based on it's distance from the shell.
	            float damage = CalculateDamage (targetRigidbody.position);
	
	            // Deal this damage to the tank.
	            targetHealth.TakeDamage (damage);
	        }
	
	        // Unparent the particles from the shell.
	        m_ExplosionParticles.transform.parent = null;
	
	        // Play the particle system.
	        m_ExplosionParticles.Play();
	
	        // Play the explosion sound effect.
	        m_ExplosionAudio.Play();
	
	        // Once the particles have finished, destroy the gameobject they are on.
	        Destroy (m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);
	
	        // Destroy the shell.
	        Destroy (gameObject);
	    }
	
	
	    private float CalculateDamage (Vector3 targetPosition)
	    {
	        // Create a vector from the shell to the target.
	        Vector3 explosionToTarget = targetPosition - transform.position;
	
	        // Calculate the distance from the shell to the target.
	        float explosionDistance = explosionToTarget.magnitude;
	
	        // Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
	        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
	
	        // Calculate damage as this proportion of the maximum possible damage.
	        float damage = relativeDistance * m_MaxDamage;
	
	        // Make sure that the minimum damage is always 0.
	        damage = Mathf.Max (0f, damage);
	
	        return damage;
	    }
	}

```

- Mens man fortsatt har `Shell` valgt, dra child objektet `ShellExplosion` til `ExplosionParticles` og `ExplosionAudio` variablene
- Sett `TankMask` variablen til `Players`
- Dra `Shell` objektet til `Prefabs` folderen.
- Slett `Shell` objektet fra hierarkiet og lagre scenen.

## 6 Skyting

- Velg Tanksen vår i hierarkiet
- Høyre-klikk på Tanksen vår og velg `Crate Empty`, gi denne navnet `FireTransform`
- Sett posisjonen til (0, 1.7, 1.35) og rotasjonen til (350, 0, 0)
- Høyreklikk på `Canvas` objektet vårt i hierarkiet og velg `UI > Slider`
- Gi den nye slideren navnet `AimSlider`
##
- Hold inne `Alt` og klikk på pilen ved siden av `AimSlider` for å vise alle children av objektet
- Slett `Background` og `Handle Slider Area` fra objektet
- Skru av `Interactable`
- Sett `Transition` til `None`
- Sett `Direction` til `Bottom to Top`
- Sett `Min Value` til 15 og `Max Value` til 30
- Velg både `AimSlider` og `Fill Area`
- I deres `Rect Transform` klikk på `Anchor Preset` og hold inn `Alt` mens du klikker på den preseten ned i høyre hjørne, `Stretch`
- Åpne `Fill Area` og velg `Fill`
- På `Rect Transform` sett `Height` til 0
- Under `Fill` komponenten forandre `Source Image` til `Aim Arrow`

##
- Velg `AimSlider` og bruk `Rect Tool`, du kan velge det ved å trykke på `T` på tastaturet
- Dra firkanten slik at den er cirka like bred som tanksen, og litt lengre foran tanksen
- Finn `TankShooting` scriptet i `Scripts/Tank` og dra det over på Tanksen vår
- Åpne Scriptet

```

	using UnityEngine;
	using UnityEngine.UI;
	
	public class TankShooting : MonoBehaviour
	{
	    public int m_PlayerNumber = 1;              // Used to identify the different players.
	    public Rigidbody m_Shell;                   // Prefab of the shell.
	    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
	    public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
	    public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
	    public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
	    public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
	    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
	    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
	    public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.
	
	
	    private string m_FireButton;                // The input axis that is used for launching shells.
	    private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
	    private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
	    private bool m_Fired;                       // Whether or not the shell has been launched with this button press.
	
	
	    private void OnEnable()
	    {
	        // When the tank is turned on, reset the launch force and the UI
	        m_CurrentLaunchForce = m_MinLaunchForce;
	        m_AimSlider.value = m_MinLaunchForce;
	    }
	
	
	    private void Start ()
	    {
	        // The fire axis is based on the player number.
	        m_FireButton = "Fire" + m_PlayerNumber;
	
	        // The rate that the launch force charges up is the range of possible forces by the max charge time.
	        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
	    }
	
	
	    private void Update ()
	    {
	        // The slider should have a default value of the minimum launch force.
	        m_AimSlider.value = m_MinLaunchForce;
	
	        // If the max force has been exceeded and the shell hasn't yet been launched...
	        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
	        {
	            // ... use the max force and launch the shell.
	            m_CurrentLaunchForce = m_MaxLaunchForce;
	            Fire ();
	        }
	        // Otherwise, if the fire button has just started being pressed...
	        else if (Input.GetButtonDown (m_FireButton))
	        {
	            // ... reset the fired flag and reset the launch force.
	            m_Fired = false;
	            m_CurrentLaunchForce = m_MinLaunchForce;
	
	            // Change the clip to the charging clip and start it playing.
	            m_ShootingAudio.clip = m_ChargingClip;
	            m_ShootingAudio.Play ();
	        }
	        // Otherwise, if the fire button is being held and the shell hasn't been launched yet...
	        else if (Input.GetButton (m_FireButton) && !m_Fired)
	        {
	            // Increment the launch force and update the slider.
	            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
	
	            m_AimSlider.value = m_CurrentLaunchForce;
	        }
	        // Otherwise, if the fire button is released and the shell hasn't been launched yet...
	        else if (Input.GetButtonUp (m_FireButton) && !m_Fired)
	        {
	            // ... launch the shell.
	            Fire ();
	        }
	    }
	
	
	    private void Fire ()
	    {
	        // Set the fired flag so only Fire is only called once.
	        m_Fired = true;
	
	        // Create an instance of the shell and store a reference to it's rigidbody.
	        Rigidbody shellInstance =
	            Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
	
	        // Set the shell's velocity to the launch force in the fire position's forward direction.
	        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; ;
	
	        // Change the clip to the firing clip and play it.
	        m_ShootingAudio.clip = m_FireClip;
	        m_ShootingAudio.Play ();
	
	        // Reset the launch force.  This is a precaution in case of missing button events.
	        m_CurrentLaunchForce = m_MinLaunchForce;
	    }
	}
```

##
- Nå må vi sette alle `public` variablene i `TankShooting` scriptet vårt
- Finn `Shell` fra prefabs og dra det til `Shell` variablen
- Dra `FireTransform` komponenten til `FireTransform` variablen
- Finn `AimSlider`, som er en child av `Canvas` og dra den til `AimSlider` variablen
- Den andre (tomme) `AudioSourcen` som ligger på Tanksen vår drar vi til `Shooting Audio` variablen
- Sett `Charging Clip` variablen til `ShotCharging`
- Sett `Fire Clip` variablen til `ShotFiring`
- Klikk `Apply` øverst i Tanks objektet vårt og lagre scenen

# Tanksen vår er nå ferdig, test den ut :)

## 7 Game Manager

- Lag to nye `Empty GameObjects` i hierarkiet og kall dem for `SpawnPoint1` og `SpawnPoint2`
- Sett posisjonen til første spawn til (-3, 0, 30) og andre og rotatsjon til (0, 180, 0)
- Sett posisjonen til andre spawn til (13, 0, -5), rotasjon skal stå på (0, 0, 0)
- Trykk på `Gizmoen` til SpawnPoint1 og forandre den til Blå
- Trykk på `Gizmoen` til SpawnPoint2 og forandre den til Rød
- Lag et nytt Canvas i hierarkiet og gi det navnet `MessageCanvas`
- Klikk på 2D knappen øverst i scenen vår, og posisjoner deg så du kan se hele Canvaset
- Høyreklikk på `MessageCanvas` og legg til et `Text` objekt
- På `RectTransform` komponenten til `Text` objektet, sett Anchor for X og Y til Min: 0,1 og Max: 0,9
- Skriv inn `TANKS!!` i tekstfeltet til `Text` objektet
- Forandre `Font` til `BowlbyOne-regular` ved å bruke sirkelen ved siden av `Font` feltet
- Forandre alignment til senter og midten
- Klikk på `Enable Best Fit`, sett max size til 60 og forandre farge til hvit

- Gå til `CameraRig` og forandre `Size` i target variablen til 0 og trykk enter
- Lag et nytt `Empty GameObject` og kall objektet `GameManager`
- Finn `GameManager` objektet i `Scripts/GameManager` og dra det til `GameManager` objektet
- Dra inn alle objektene som skal være i `Public` variablene til `GameManager` scriptet
- Åpne `Tanks` variablen (array) og sett størrelsen til 2
- Dra `SpawnPoint1` til Element 0 og forandre farge, gjør det samme for Element 1
- Lagre Scenen din

