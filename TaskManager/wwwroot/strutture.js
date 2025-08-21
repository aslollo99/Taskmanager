// caricamento iniziale
document.addEventListener('DOMContentLoaded', () => {
    loadStructures();
});

function goBack() {
    window.location.href = "home.html"; // o la pagina a cui vuoi tornare
}

async function loadStructures() {
    try {
        const response = await fetch("/struttura"); // endpoint GET strutture
        if (!response.ok) throw new Error("Errore nel caricamento delle strutture");
        const data = await response.json();
        renderTable(data);
    } catch (err) {
        alert(err.message);
    }
}

function renderTable(structures) {
    const tbody = document.getElementById("structuresTable");
    tbody.innerHTML = "";
    structures.forEach(s => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${s.id}</td>
            <td>${s.name}</td>
            <td>${s.indirizzo}</td>
            <td>
                <button class="btn btn-warning btn-sm me-1" onclick="editStructure(${s.id})">Modifica</button>
                <button class="btn btn-danger btn-sm" onclick="deleteStructure(${s.id})">Elimina</button>
            </td>
        `;
        tbody.appendChild(tr);
    });
}

function openAddModal() {
    document.getElementById("modalTitle").innerText = "Aggiungi Struttura";
    document.getElementById("structureId").value = "";
    document.getElementById("structureName").value = "";
    document.getElementById("structureAddress").value = "";
}

async function editStructure(id) {
    try {
        const response = await fetch(`/struttura/${id}`); // GET struttura per ID
        if (!response.ok) throw new Error("Struttura non trovata");
        const s = await response.json();
        document.getElementById("modalTitle").innerText = "Modifica Struttura";
        document.getElementById("structureId").value = s.id;
        document.getElementById("structureName").value = s.name;
        document.getElementById("structureAddress").value = s.indirizzo;
        new bootstrap.Modal(document.getElementById('structureModal')).show();
    } catch (err) {
        alert(err.message);
    }
}

async function saveStructure() {
    const id = document.getElementById("structureId").value;
    const name = document.getElementById("structureName").value;
    const indirizzo = document.getElementById("structureAddress").value;

    try {
        let response;
        if (id) {
            // PUT modifica struttura
            response = await fetch(`/struttura/${id}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ name, indirizzo })
            });
        } else {
            // POST nuova struttura
            response = await fetch("/struttura/add", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ name, indirizzo })
            });
        }
        if (!response.ok) throw new Error("Errore nel salvataggio della struttura");

        bootstrap.Modal.getInstance(document.getElementById('structureModal')).hide();
        loadStructures();
    } catch (err) {
        alert(err.message);
    }
}

async function deleteStructure(id) {
    if (!confirm("Sei sicuro di voler eliminare questa struttura?")) return;
    try {
        const response = await fetch(`/struttura/${id}`, { method: "DELETE" });
        if (!response.ok) throw new Error("Errore nell'eliminazione");
        loadStructures();
    } catch (err) {
        alert(err.message);
    }
}
