using UnityEngine;
using System.Collections;

public class CubeControl : MonoBehaviour {

	// En linje som starter med to skråstreker som denne betyr at det er en kommentar.
	// Kommentarer er ikke en del av koden, og man kan skrive små notater til seg selv
	// eller til andre som skal samarbeide om å skrive koden.

	/* Man kan også bruke skråstrek og stjerne for å skrive en kommentar
	 * over flere linjer uten å måtte a to skråstreker på starten av hver linje.
	 */

	// Public betyr at det som står etter kan nåes fra hvor som helst innenfor unity.
	// Float er noe vi kaller en datatype. Float betyr at det som har navnet speed er et desimaltall.
	// Speed er navnet vi har gitt desimaltallet vårt.
	public float speed = 10f;
	public bool gravity = false;

	// Dette er en funksjon, en funksjon er en samling av kode som vi kan få til 
	// å kjøre en annen plass i koden vår ved å si Update(). Dette er kjent som 
	// et funksjonskall eller "å kalle en funksjon".
	// Unity kaller denne funksjonen automatisk hvert eneste bilde som vises i spillet
	// vårt, så alt som står inni klammeparantesene { } til Update() vil skjer flere ganger
	// per sekund.
	// Det er vanlig at spill viser mellom 30 og 60 biler hvert eneste sekund, altså skjer alt som
	// står inni Update() ca 30 til over 120 ganger per sekund, avhengig av hvor bra datamaskinen er.
	void Update () {

		// Disse to linjene sier at kuben skal rotere med farten speed i to forskjellige retninger.
		// Time.deltaTime betyr tiden det tok å gjøre ferdig forrige bilde i spillet vårt.
		// Hvis jeg har en dårlig datamaskin og det tok lang tid å gjøre ferdig forrige bilde, så
		// betyr det at farten må være større for at alt skal gå like fort på alle datamaskiner uansett hvor
		// bra eller dårlige de er.
		transform.Rotate(Vector3.up, speed * Time.deltaTime);
		transform.Rotate(Vector3.right, speed * Time.deltaTime);


		// Dette gjør at man kan slå av og på gravitasjonen til kuben.
		// Hvis gravitasjonen er på, og man slår den av, så vil kuben hoppe tilbake til sin første
		// posisjon.
		if (gravity == false)
		{
			if(GetComponent<Rigidbody>().isKinematic == false)
			{
				GetComponent<Rigidbody>().isKinematic = true;
            }
			transform.position = new Vector3(0f, 5.5f, 0f);
		}
		else
		{
			if (GetComponent<Rigidbody>().isKinematic == true)
			{
				GetComponent<Rigidbody>().isKinematic = false;
			}
		}
		
	}
}
