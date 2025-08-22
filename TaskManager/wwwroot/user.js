// Greeting + logout
document.addEventListener("DOMContentLoaded", () => {
    const name = localStorage.getItem("fullname");
    if (name) document.getElementById("userGreeting").textContent = `Ciao ${name}`;
    document.getElementById("logoutBtn").addEventListener("click", () => {
        localStorage.removeItem("fullname");
        localStorage.removeItem("role");
        window.location.href = "login.html";
    });

    loadUsers();
});

function goBack() {
    window.location.href = "home.html"; // o la pagina a cui vuoi tornare
}


async function loadUsers() {
    const res = await fetch("/user");
    const data = await res.json();
    renderUsers(data);
}

function renderUsers(users) {
    const tbody = document.getElementById("usersTable");
    tbody.innerHTML = "";
    users.forEach(u => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
      <td>${u.id}</td>
      <td>${u.fullName || u.FullName || u.fullname}</td>
      <td>${u.username || u.Username}</td>
      <td><span class="badge ${((u.role||u.Role)==='admin')?'bg-primary':'bg-secondary'}">${u.role || u.Role}</span></td>
      <td>
        <button class="btn btn-warning btn-sm me-1" onclick="openEditUserModal(${u.id})">Modifica</button>
        <button class="btn btn-warning btn-sm me-1" onclick="openQrCodeModal(${u.id},'${u.fullname.replace(/'/g, "\\'")}')">QrCode</button>
        <button class="btn btn-danger btn-sm" onclick="deleteUser(${u.id})">Elimina</button>
      </td>
    `;
        tbody.appendChild(tr);
    });
}

function openAddUserModal() {
    document.getElementById("userModalTitle").textContent = "Aggiungi Utente";
    document.getElementById("userId").value = "";
    document.getElementById("fullName").value = "";
    document.getElementById("username").value = "";
    document.getElementById("password").value = "";
    document.getElementById("role").value = "user";
}

async function openEditUserModal(id) {
    const res = await fetch(`/user/${id}`);
    if (!res.ok) return alert("Utente non trovato");
    const u = await res.json();
    document.getElementById("userModalTitle").textContent = "Modifica Utente";
    document.getElementById("userId").value = u.id;
    document.getElementById("fullName").value = u.fullName || u.FullName || u.fullname;
    document.getElementById("username").value = u.username || u.Username;
    document.getElementById("password").value = ""; // vuoto: non cambiare
    document.getElementById("email").value = u.email || u.Email;
    document.getElementById("role").value = (u.role || u.Role) ?? "user";
    new bootstrap.Modal(document.getElementById("userModal")).show();
}

async function saveUser() {
    const id = document.getElementById("userId").value;
    const FullName = document.getElementById("fullName").value.trim();
    const Username = document.getElementById("username").value.trim();
    const Password = document.getElementById("password").value;
    const Email = document.getElementById("email").value.trim();
    const Role = document.getElementById("role").value;

    if (!FullName || !Username) return alert("Nome completo e Username sono obbligatori");

    let body = { FullName, Username, Role, Email };
    // In modifica invia Password solo se è stato inserito
    if (!id || Password) body.Password = Password;

    let res;
    if (id) {
        res = await fetch(`/user/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body)
        });
    } else {
        res = await fetch(`/user/add`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body)
        });
    }
    if (!res.ok) return alert("Errore nel salvataggio utente");
    bootstrap.Modal.getInstance(document.getElementById("userModal")).hide();
    loadUsers();
}

async function deleteUser(id) {
    if (!confirm("Eliminare questo utente?")) return;
    const res = await fetch(`/user/${id}`, { method: "DELETE" });
    if (!res.ok) return alert("Errore nell'eliminazione");
    loadUsers();
}

let currentUserId = null;
async function openQrCodeModal(userId, fullName) {
    currentUserId = userId;
    document.getElementById("qrcodeUserName").textContent = fullName;

    // carica i QRCode per l'utente
    await loadQrCodes(userId);

    // mostra la modale con Bootstrap
    new bootstrap.Modal(document.getElementById("qrcodeModal")).show();
}


// Carica i QRCode di un utente
async function loadQrCodes(userId) {
    try {
        let res = await fetch(`/user/${userId}/qrcodes`);
        let qrcodes = await res.json();

        let tbody = document.getElementById("qrcodeTableBody");
        tbody.innerHTML = "";
        qrcodes.forEach(qr => {
            let tr = document.createElement("tr");

            tr.innerHTML = `
                <td>${qr.codeValue}</td>
                <td>${qr.isActive ? "✅" : "❌"}</td>
                <td>
                  <button onclick="toggleQrCode(${qr.id}, ${!qr.isActive})">
                    ${qr.isActive ? "Disattiva" : "Attiva"}
                  </button>
                  <button onclick="deleteQrCode(${qr.id})">Elimina</button>
                </td>
                <td>
                    <button class="btn btn-warning btn-sm me-1" onclick="openAccessModal(${qr.id},'${qr.codeValue.replace(/'/g, "\\'")}')">Gestione Accessi</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (err) {
        alert("Errore caricamento QRCode: " + err.message);
    }
}

// Salva nuovo QRCode
async function saveQrCode(e) {
    e.preventDefault();
    let body = {
        codeValue: document.getElementById("qrCodeValue").value,
        isActive: document.getElementById("qrIsActive").checked
    };

    try {
        let res = await fetch(`/user/${currentUserId}/qrcodes`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body)
        });
        if (!res.ok) throw new Error("Errore salvataggio QRCode");

        document.getElementById("addQrCodeForm").reset();
        loadQrCodes(currentUserId);
    } catch (err) {
        alert(err.message);
    }
}

// Attiva/disattiva
async function toggleQrCode(id, newStatus) {
    try {
        let res = await fetch(`/qrcode/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ isActive: newStatus })
        });
        if (!res.ok) throw new Error("Errore aggiornamento QRCode");
        loadQrCodes(currentUserId);
    } catch (err) {
        alert(err.message);
    }
}

// Elimina QRCode
async function deleteQrCode(id) {
    if (!confirm("Sei sicuro di voler eliminare questo QRCode?")) return;
    try {
        let res = await fetch(`/qrcode/${id}`, { method: "DELETE" });
        if (!res.ok) throw new Error("Errore eliminazione QRCode");
        loadQrCodes(currentUserId);
    } catch (err) {
        alert(err.message);
    }
}

async function openAccessModal(qrCodeId, codeValue) {
    // chiudi modale QRCode
    const qrModal = bootstrap.Modal.getInstance(document.getElementById("qrcodeModal"));
    qrModal.hide();

    // carica dati come già fatto
    document.getElementById("accessQrCodeId").value = qrCodeId;
    document.getElementById("accessQrCodeValue").textContent = codeValue;

    const resDoors = await fetch("/door");
    const doors = await resDoors.json();

    const resAccess = await fetch(`/accessright/byqrcode/${qrCodeId}`);
    const rights = await resAccess.json();

    const tbody = document.getElementById("accessTableBody");
    tbody.innerHTML = "";
    doors.forEach(d => {
        const existing = rights.find(r => r.doorId === d.id);
        const checked = existing ? "checked" : "";
        const validFrom = existing?.validFrom?.split("T")[0] || "";
        const validTo = existing?.validTo?.split("T")[0] || "";

        tbody.innerHTML += `
            <tr>
                <td>${d.name}</td>
                <td><input type="checkbox" class="form-check-input" data-doorid="${d.id}" ${checked}></td>
                <td><input type="date" class="form-control" data-doorid="${d.id}" data-field="from" value="${validFrom}"></td>
                <td><input type="date" class="form-control" data-doorid="${d.id}" data-field="to" value="${validTo}"></td>
            </tr>
        `;
    });

    // apri modale accessi
    const accessModal = new bootstrap.Modal(document.getElementById("accessModal"));
    accessModal.show();

    // quando chiudi Accessi → riapri QRCode
    document.getElementById("accessModal").addEventListener("hidden.bs.modal", () => {
        qrModal.show();
    }, { once: true });
}

async function saveAccessRights() {
    const qrCodeId = document.getElementById("accessQrCodeId").value;

    // Costruisci lista da tabella
    const rows = document.querySelectorAll("#accessTableBody tr");
    let rights = [];
    rows.forEach(r => {
        const doorId = r.querySelector("input[type='checkbox']").dataset.doorid;
        const checked = r.querySelector("input[type='checkbox']").checked;
        if (checked) {
            const validFrom = r.querySelector("input[data-field='from']").value || null;
            const validTo = r.querySelector("input[data-field='to']").value || null;
            rights.push({
                qrCodeId,
                doorsId: parseInt(doorId),
                validFrom,
                validTo
            });
        }
    });

    // Chiamata BE
    const res = await fetch(`/accessright/byqrcode/${qrCodeId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(rights)
    });

    if (res.ok) {
        alert("Accessi salvati!");
        bootstrap.Modal.getInstance(document.getElementById("accessModal")).hide();
    } else {
        alert("Errore nel salvataggio accessi");
    }
}


