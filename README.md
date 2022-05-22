# Link2Me

# Introduzione

Link2Me è una WebAPI con funzionalità di social networking che permette all'utente di inserire in una friend list i colleghi del proprio network aziendale.

# Tecnologie utilizzate

- Target Framework .NET 6.0
- Entity Framework 6
- Razor
- jQuery 3.6.0
- Microsoft SQL Server 15

# Descrizione dell'architettura

L'applicazione è stata sviluppata seguendo il pattern MVC.
La separazione tra model e controller avviene grazie a Entity Framework, mentre le view sono separate dai controller tramite chiamate AJAX.

# Struttura database

![Image](https://github.com/DZardo/Link2Me/blob/master/db_schema.png?raw=true)

- La tabella `Users` rappresenta le entità utilizzate durante il login. Idealmente le utenze sono create dall'amministratore e assegnate univocamente a un Employee. `IsAdmin` indica se l'utenza è in possesso dei privilegi da amministratore (determinerà la visibilità della pagina con le funzionalità da admin). È stato separato dalla logica dell'employee perché non è obbligatorio per un utente essere parte del network aziendale (es. account per amministrazione, account di test, account in comune).
- `Departments` contiene una lista statica di dipartimenti a cui gli employees vengono assegnati.
- `Employees` è l'entità richiesta dalla consegna. Contiene tutte le informazioni personali compresa la posizione, rappresentata nella tabella da una stringa formato JSON dell'oggetto Position (l'implementazione del tipo Geography mi ha dato qualche problema e ho optato per questa soluzione).
- `Friends` rappresenta le singole amicizie. `UserEmployeeId` è l'id dell'employee relativo all'account che aggiunge un collega alla propria friend list, FriendId è l'id dell'employee aggiunto. Alla creazione dell'amicizia viene registrato un timestamp con data e ora del momento esatto.

# Struttura dei controller

Il testing iniziale degli endpoint è stato eseguito con Postman, qui il link con la collezione dei vari test:

[Postman collection](https://www.postman.com/collections/730bc5c5de95aae85364)

Le chiamate ad uso esclusivo degli account di amministrazione (es. creazione, modifica o cancellazione di un employee) hanno come requisito il parametro "securityKey" da avere nella header di ogni request, che idealmente sarà una stringa crittata che verrà confrontata lato backend con una chiave corrispondente salvata su database prima di eseguire. Nella implementazione corrente si limita a verificare che corrisponda a una semplice stringa hardcoded nella classe del controller.

## Department
- `GET api/departments`
Restituisce la lista di tutti i dipartimenti
- `GET api/departments/{departmentId}`
Restituisce il dipartimento specificato

## Employee
- `GET api/employees`
Restituisce la lista di tutti gli impiegati. Lo stesso endpoint viene utilizzato per una ricerca filtrata se i parametri corretti vengono passati. Ad esempio:
- `GET api/employees?firstName={firstName}&lastName={lastName}&departmentId={departmentId}`
Restituisce la lista degli impiegati filtrata secondo nome, cognome e dipartimento assegnato.
- `GET api/employees/api/employees?firstName=&lastName=&departmentId={departmentId}`
Restituisce la lista degli impiegati filtrata secondo dipartimento assegnato.

- `GET api/employees/{employeeId}`
Restituisce l'impiegato specificato
- `POST api/employees`
Inserisce un nuovo impiegato. I dati dell'impiegato vengono passati nel body della richiesta. Il parametro `securityKey` deve essere presente nella header della richiesta.
- `PUT api/employees/{employeeId}`
Aggiorna i dati dell'impiegato specificato. I dati dell'impiegato vengono passati nel body della richiesta.
- `DELETE api/employees/{employeeId}`
Elimina il record relativo all'impiegato specificato. Il parametro `securityKey` deve essere presente nella header della richiesta.

- `PUT api/employees/setposition/{employeeId}`
Aggiorna la posizione dell'impiegato specificato. Latitudine e longitudine vengono passati nel body della richiesta. Idealmente dovrebbe essere un processo da richiamare periodicamente in automatico.

## Friend
- `GET api/friends?userEmployeeId={userEmployeeId}`
Restituisce la friend list dell'utente specificato. Il parametro `userEmployeeId` è relativo all'utente collegato e viene preso dalla sessione.
- `POST api/friends/{friendId}`
Aggiunge l'impiegato specificato alla propria friend list. Il parametro `userEmployeeId` è nel body della richiesta.
- `DELETE api/friends/{friendId}`
Rimuove l'impiegato specificato dalla propria friend list. Il parametro `userEmployeeId` è nel body della richiesta.

- `GET api/friends/getdistance/{userEmployeeId}/{friendId}`
Restituisce la distanza tra un impiegato (idealmente quello relativo all'utente loggato) e un altro appartenente alla friend list del primo.

## User
- `POST api/login`
L'endpoint richiamato nella procedura di login. Verifica le credenziali e restituisce un oggetto con i dati relativi all'utente, tra cui la securityKey se l'utente è un amministratore.

