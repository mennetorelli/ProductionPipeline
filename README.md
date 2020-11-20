# Production Pipeline

# Utilizzo

- WASD o frecce: spostamento camera
- Tasto destro del mouse + spostamento mouse: rotazione camera
- Click del mouse con tasto sinistro su una risorsa o un componente della pipeline: visualizzazione del pannello con i dettagli.
  Cliccando a vuoto il pannello con i dettagli si disattiva.

# Scelte architetturali

## Risorse

Ogni risorsa ha un componente `Resource`, in cui è definita una lista di oggetti di tipo `ResourceProperties`, composti a loro volta da:

- `Type: string`: tipo della risorsa (Base, Body, Detail). Viene inizializzato dal nome del GameObject.
- `ID: string`: id alfanumerico generato casualmente (la lunghezza di default dell'id è di 6 caratteri, ma può essere modificata).
- `Value: KeyValuePair<Type, string>`: valore della risorsa.
  Dato che questo valore può essere un `int` (come per Base e Detail) o un `char` (come per Body),
  si è optato per codificarlo in modo semplice (con una stringa), tenendo traccia del tipo in modo tale da poterlo decodificare se necessario
  (come nel caso del controllo che esegue il Quality Assurance).
  Questa scelta garantisce flessibilità, perchè altri componenti potrebbero eseguire operazioni diverse, a seconda del tipo di dato specificato.
- `Color: Color`: colore della risorsa, i cui valori R,G,B sono generati casualmente.

Quando una risorsa è composta da più risorse (come Base+Base), la lista di `ResourceProperties` contiene i dettagli di tutte le risorse da cui è composta,
mentre in queste ultime non è presente il componente `Resource`.

## Componenti della pipeline

Ogni componente della pipeline estende la classe astratta `PipelineComponent`, in modo tale da definire delle caratteristiche comuni a tutti i componenti.
Per esempio, ogni elemento ha un riferimento a una lista di `GameObject` (chiamata `Next`) che sono immediatamente dopo di lui nella pipeline.
Solitamente `Next` contiene solo un elemento, però questa soluzione prende in considerazione elementi come Flow Splitter e Quality Assurance,
che indicano delle biforcazioni nel flusso della pipeline.

`PipelineComponent` fornisce due metodi principali:

- `Use(GameObject resource)`: metodo astratto, serve per definire come ogni componente della pipeline utilizza la risorsa corrente.
  Per esempio, il buffer immagazzina la risorsa (inserendola in una lista), l'assembler la compone ad altre risorse,
  il flow splitter semplicemente chiama il metodo `GoToNext`, ecc.
- `GoToNext(GameObject resource)`: sposta il `GameObject` della risorsa alla posizione del prossimo elemento
  (positione ricavata dalla transform dell'elemento stesso o, opzionalmente, da un transform differente referenziata dall'inspector).
  In questo modo è garantita la flessibilità della pipeline,
  dato che lo spostamento delle risorse non dipende dalla rotazione/posizione degli vari elementi,
  ma solo dalle loro transform. Una volta finito lo spostamento viene chiamato il metodo `Use` del prossimo elemento.
  Il metodo è virtual perché il comportamento di default è prendere il primo (e unico) elemento di `Next`,
  però è possibile eseguirne l'override per implementare comportamenti diversi (come nel caso di Flow Splitter e Quality Assurance).

Riassumendo, una risorsa all'interno della pipeline viene quindi utilizzata dagli elementi della pipeline tramite il metodo `Use`
e passata da un elemento all'altro della pipeline col metodo `GoToNext`, seguendo un alternarsi continuo di queste due azioni.

### Source producer

Tramite l'inspector si deve specificare il prefab della risorsa da creare e l'intervallo di tempo necessario per la creazione.
Il metodo `Use` genera la risorsa, e viene chiamato ad ogni intervallo di tempo stabilito.

### Source receiver

Questo componente non è configurabile, il metodo `Use` si occupa soltanto della distruzione di un qualsiasi tipo di risorsa in ingresso.
La lista `Next` è vuota.

### Conveyor

Questo componente è costituito da una serie di "blocchi" (transform) che la risorsa attraversa,
prima di passare al componente successivo della pipeline col metodo `GoToNext`.
Dall'inspector è possibile specificare l'ordine dei blocchi tramite una lista di transform,
se questa non viene specificata allora viene considerato l'ordine dato da tutti i figli dell'elemento con il componente `Conveyor`.

### Flow splitter

Per questo componente deve essere fornito un valore float per ogni elemento definito in `Next`,
che indica la probabilità che la risorsa venga inviata all'elemento stesso.
(La lista indica una distrubuzione di probabilità, quindi la somma dei valori deve essere 1).

### Assembler

Devono essere configurati dall'inspector la lista di prefab delle risorse in ingresso, e il prefab della risorsa da assemblare.
L'utilizzo di una lista per le risorse in ingresso permette che la risorsa da assemblare sia composta da un numero arbitrario di risorse.
Si può personalizzare tramite l'inspector il tempo necessario per assemblare la risorsa di output,
che scatta una volta che tutte le sottorisorse sono arrivate.

Il prefab della risorsa da assemblare è composto dalle sottorisorse "vuote", nel senso che non hanno un componente `Resource`
e i colori dei materiali non sono inizializzati.

Per ogni risorsa che viene passata al metodo `Use`, l'Assembler controlla se non è già arrivata.
Nel caso, integra le sue caratteristiche nella risorsa da assemblare, altrimenti viene scartata.

Una volta che la risorsa di output è stata assemblata, viene considerata come risorsa unica, con un solo componente `Resource`
che contiene nella lista `ResourceProperties` tutte le proprietà delle risorse di cui è composta.

### Buffer

Il metodo `Use` aggiunge la risorsa in arrivo ad una lista. Se questa lista non è vuota, allora il buffer fa partire un timer e, allo scadere di questo,
l'ultima risorsa inserita nella lista viene rilasciata al prossimo componente della pipeline. Si può personalizzare l'intervallo di tempo tramite inspector.

### Quality Assurance

Il componente `QualityAssurance` implementa una classe astratta `QualityAssuranceBase`,
in modo tale da permettere l'implementazione di comportamenti specifici per i vari elementi.
Nel caso di Q1, vengono scartate tutte le risorse la cui somma delle variabili `Value` di ogni elemento della lista `ResourceProperties` è minore di 100.
