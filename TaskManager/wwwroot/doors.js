let doors = [];
let structures = [];

function goBack() {
    window.location.href = "home.html"; // o la pagina a cui vuoi tornare
}

document.getElementById("logoutBtn").addEventListener("click", () => {
    localStorage.removeItem("fullname");
    window.location.href = "login.html";
});

async function loadDoors() {
    const response = await fetch("/door");
    doors = await response.json();
    renderDoors();
}

async function loadStructures() {
    const response = await fetch("/struttura");
    structures = await response.json();
    const select = document.getElementById("doorStructure");
    select.innerHTML = "";
    structures.forEach(s => {
        const opt = document.createElement("option");
        opt.value = s.id;
        opt.textContent = s.name;
        select.appendChild(opt);
    });
}

function renderDoors() {
    const tbody = document.getElementById("doorsTable");
    tbody.innerHTML = "";
    doors.forEach(d => {
        const strutturaName = structures.find(s => s.id === d.strutturaId)?.name || "N/A";
        const activeLabel = d.isActive ? "✅" : "❌";
        const row = `
            <tr>
                <td>${d.id}</td>
                <td>${d.name}</td>
                <td>${strutturaName}</td>
                <td>${activeLabel}</td>
                <td>
                    <button class="btn btn-sm btn-warning" onclick="openEditDoorModal(${d.id})">Modifica</button>
                    <button class="btn btn-sm btn-danger" onclick="deleteDoor(${d.id})">Elimina</button>
                </td>
            </tr>`;
        tbody.innerHTML += row;
    });
}

function openAddDoorModal() {
    document.getElementById("doorId").value = "";
    document.getElementById("doorName").value = "";
    document.getElementById("doorLocation").value = "";
    document.getElementById("doorIsActive").checked = true;
    document.getElementById("doorModalTitle").textContent = "Aggiungi Porta";
    loadStructures();
}

function openEditDoorModal(id) {
    const door = doors.find(d => d.id === id);
    if (!door) return;
    document.getElementById("doorId").value = door.id;
    document.getElementById("doorName").value = door.name;
    document.getElementById("doorLocation").value = door.location;
    document.getElementById("doorIsActive").checked = door.isActive;
    document.getElementById("doorStructure").value = door.strutturaId;
    document.getElementById("doorModalTitle").textContent = "Modifica Porta";
    const modal = new bootstrap.Modal(document.getElementById("doorModal"));
    modal.show();
}

async function saveDoor() {
    const id = document.getElementById("doorId").value;
    const name = document.getElementById("doorName").value;
    const location = document.getElementById("doorLocation").value;
    const strutturaId = document.getElementById("doorStructure").value;
    const isActive = document.getElementById("doorIsActive").checked;

    let response;
    if (id) {
        response = await fetch(`/door/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ name, strutturaId, location, isActive })
        });
    } else {
        response = await fetch("/door/add", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ name, strutturaId, location, isActive })
        });
    }

    if (response.ok) {
        loadDoors();
        bootstrap.Modal.getInstance(document.getElementById("doorModal")).hide();
    } else {
        alert("Errore nel salvataggio porta");
    }
}

async function deleteDoor(id) {
    if (!confirm("Sei sicuro di voler eliminare questa porta?")) return;
    const response = await fetch(`/door/${id}`, { method: "DELETE" });
    if (response.ok) {
        loadDoors();
    } else {
        alert("Errore nell'eliminazione");
    }
}

window.onload = () => {
    loadStructures().then(loadDoors);
};
