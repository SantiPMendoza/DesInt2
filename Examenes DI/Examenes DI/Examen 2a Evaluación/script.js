
document.addEventListener("DOMContentLoaded", () => {
    renderAllTables();
    populateUserDropdown();
});

// Función para renderizar tablas con botones de acción
function renderTable(tableId, data, columns, deleteFunction, extraActions = null) {
    const tableBody = document.getElementById(tableId);
    tableBody.innerHTML = "";
    data.forEach(item => {
        const row = document.createElement("tr");

        row.innerHTML = columns.map(col => `<td>${Array.isArray(item[col]) ? item[col].join(", ") : item[col]}</td>`).join("");

        // Agregar botón de eliminar
        const actions = document.createElement("td");
        actions.classList.add("action-buttons");
        const deleteBtn = document.createElement("button");
        deleteBtn.innerHTML = "🗑️";
        deleteBtn.classList.add("delete");
        deleteBtn.onclick = () => window[deleteFunction](item.id);
        actions.appendChild(deleteBtn);

        // Agregar botones extra (añadir y quitar productos en pedidos)
        if (extraActions) {
            const addProductBtn = document.createElement("button");
            addProductBtn.innerHTML = "➕";
            addProductBtn.onclick = () => addProductToOrder(item.id);
            actions.appendChild(addProductBtn);

            const removeProductBtn = document.createElement("button");
            removeProductBtn.innerHTML = "➖";
            removeProductBtn.onclick = () => removeProductFromOrder(item.id);
            actions.appendChild(removeProductBtn);
        }

        row.appendChild(actions);
        tableBody.appendChild(row);
    });
}

// CRUD Usuarios
function addUser() {
    const nombre = document.getElementById("userNameInput").value;
    const email = document.getElementById("userEmailInput").value;
    if (!nombre || !email) return alert("Completa los campos");

    users.push({ id: users.length + 1, nombre, email, productos: [] });
    renderAllTables();
    closeModal("userForm");
}

function deleteUser(id) {
    users.splice(users.findIndex(u => u.id === id), 1);
    renderAllTables();
}

// CRUD Productos
function addProduct() {
    const nombre = document.getElementById("productNameInput").value;
    const precio = parseFloat(document.getElementById("productPriceInput").value);
    if (!nombre || isNaN(precio)) return alert("Completa los campos");

    products.push({ id: products.length + 1, nombre, precio });
    renderAllTables();
    closeModal("productForm");
}

function deleteProduct(id) {
    products.splice(products.findIndex(p => p.id === id), 1);
    renderAllTables();
}

// CRUD Pedidos
function addOrder() {
    const fecha = document.getElementById("orderDateInput").value;
    const userId = parseInt(document.getElementById("orderUserInput").value);
    const usuario = users.find(u => u.id === userId);
    if (!fecha || !usuario) return alert("Selecciona un usuario y una fecha");

    orders.push({ id: orders.length + 1, fecha, usuario: usuario.nombre, productos: [] });
    renderAllTables();
    closeModal("orderForm");
}

function deleteOrder(id) {
    orders.splice(orders.findIndex(o => o.id === id), 1);
    renderAllTables();
}

// Asignar Productos a Pedidos
function addProductToOrder(orderId) {
    const order = orders.find(o => o.id === orderId);
    const productName = prompt("Ingrese el nombre del producto:");
    const product = products.find(p => p.nombre === productName);

    if (order && product) {
        order.productos.push(product.nombre);
        renderAllTables();
    } else {
        alert("Producto no encontrado");
    }
}

function removeProductFromOrder(orderId) {
    const order = orders.find(o => o.id === orderId);
    if (order && order.productos.length > 0) {
        order.productos.pop();
        renderAllTables();
    }
}

// Poblar Select de Usuarios en el formulario de Pedidos
function populateUserDropdown() {
    const select = document.getElementById("orderUserInput");
    select.innerHTML = "";
    users.forEach(user => {
        const option = document.createElement("option");
        option.value = user.id;
        option.textContent = user.nombre;
        select.appendChild(option);
    });
}

// Renderizado de todas las tablas
function renderAllTables() {
    renderTable("userTableBody", users, ["id", "nombre", "email"], "deleteUser");
    renderTable("productTableBody", products, ["id", "nombre", "precio"], "deleteProduct");
    renderOrderStackPanel();
}

const users = [
    { id: 1, nombre: "Juan Pérez", email: "juan@example.com" },
    { id: 2, nombre: "Ana López", email: "ana@example.com" },
    { id: 3, nombre: "Carlos Sánchez", email: "carlos@example.com" }
];

const products = [
    { id: 1, nombre: "Laptop", precio: 1200.99 },
    { id: 2, nombre: "Mouse", precio: 25.50 },
    { id: 3, nombre: "Teclado", precio: 45.00 }
];

const orders = [
    { id: 1, fecha: "2025-03-05", usuario: "Juan Pérez", productos: ["Laptop", "Mouse"] },
    { id: 2, fecha: "2025-03-06", usuario: "Ana López", productos: ["Teclado"] }
];

function showSection(sectionId) {
    document.querySelectorAll(".section").forEach(section => section.classList.remove("active"));
    document.getElementById(sectionId).classList.add("active");

    document.querySelectorAll(".sidebar ul li").forEach(li => li.classList.remove("active"));
    document.querySelector(`.sidebar ul li[onclick="showSection('${sectionId}')"]`).classList.add("active");
}

function exportData() {
    const data = { users, products, orders };
    const blob = new Blob([JSON.stringify(data, null, 2)], { type: "application/json" });
    const a = document.createElement("a");
    a.href = URL.createObjectURL(blob);
    a.download = "data.json";
    a.click();
}

function importData() {
    const fileInput = document.getElementById("importFile");
    if (fileInput.files.length === 0) return;

    const file = fileInput.files[0];
    const reader = new FileReader();
    reader.onload = event => {
        const importedData = JSON.parse(event.target.result);
        if (importedData.users && importedData.products && importedData.orders) {
            users.length = 0;
            users.push(...importedData.users);
            products.length = 0;
            products.push(...importedData.products);
            orders.length = 0;
            orders.push(...importedData.orders);
            renderAllTables();
        }
    };
    reader.readAsText(file);
}

document.addEventListener("DOMContentLoaded", () => {
    renderAllTables();
    populateUserDropdown();
});

// Función para mostrar formularios modales
function showUserForm() {
    document.getElementById("userForm").style.display = "block";
}

function showProductForm() {
    document.getElementById("productForm").style.display = "block";
}

function showOrderForm() {
    document.getElementById("orderForm").style.display = "block";
    populateUserDropdown();
}

function closeModal(formId) {
    document.getElementById(formId).style.display = "none";
}

// CRUD Usuarios
function addUser() {
    const nombre = document.getElementById("userNameInput").value;
    const email = document.getElementById("userEmailInput").value;
    if (!nombre || !email) return alert("Completa los campos");

    users.push({ id: users.length + 1, nombre, email, productos: [] });
    renderAllTables();
    closeModal("userForm");
}

function deleteUser(id) {
    const index = users.findIndex(u => u.id === id);
    if (index !== -1) {
        users.splice(index, 1);
        renderAllTables();
    }
}

// CRUD Productos
function addProduct() {
    const nombre = document.getElementById("productNameInput").value;
    const precio = parseFloat(document.getElementById("productPriceInput").value);
    if (!nombre || isNaN(precio)) return alert("Completa los campos");

    products.push({ id: products.length + 1, nombre, precio });
    renderAllTables();
    closeModal("productForm");
}

function deleteProduct(id) {
    const index = products.findIndex(p => p.id === id);
    if (index !== -1) {
        products.splice(index, 1);
        renderAllTables();
    }
}

// CRUD Pedidos
function addOrder() {
    const fecha = document.getElementById("orderDateInput").value;
    const userId = parseInt(document.getElementById("orderUserInput").value);
    const usuario = users.find(u => u.id === userId);
    if (!fecha || !usuario) return alert("Selecciona un usuario y una fecha");

    orders.push({ id: orders.length + 1, fecha, usuario: usuario.nombre, productos: [] });
    renderAllTables();
    closeModal("orderForm");
}

function deleteOrder(id) {
    const index = orders.findIndex(o => o.id === id);
    if (index !== -1) {
        orders.splice(index, 1);
        renderAllTables();
    }
}

// Asignar Productos a Pedidos
function addProductToOrder(orderId) {
    const order = orders.find(o => o.id === orderId);
    const productName = prompt("Ingrese el nombre del producto:");
    const product = products.find(p => p.nombre === productName);

    if (order && product) {
        order.productos.push(product.nombre);
        renderAllTables();
    } else {
        alert("Producto no encontrado");
    }
}

function removeProductFromOrder(orderId) {
    const order = orders.find(o => o.id === orderId);
    if (order && order.productos.length > 0) {
        order.productos.pop();
        renderAllTables();
    }
}

// Poblar Select de Usuarios en el formulario de Pedidos
function populateUserDropdown() {
    const select = document.getElementById("orderUserInput");
    select.innerHTML = "";
    users.forEach(user => {
        const option = document.createElement("option");
        option.value = user.id;
        option.textContent = user.nombre;
        select.appendChild(option);
    });
}

// Renderizar pedidos en formato StackPanel con Cards
function renderOrderStackPanel() {
    const stackPanel = document.getElementById("orderStackPanel");
    stackPanel.innerHTML = "";
    orders.forEach(order => {
        const card = document.createElement("div");
        card.classList.add("card");
        card.innerHTML = `
            <h3>Pedido ${order.id}</h3>
            <p><strong>Fecha:</strong> ${order.fecha}</p>
            <p><strong>Usuario:</strong> ${order.usuario}</p>
            <p><strong>Productos:</strong> ${order.productos.length}</p>
        `;
        card.onclick = () => openOrderSidebar(order.id);
        stackPanel.appendChild(card);
    });
}

// Abrir menú lateral con detalles del pedido seleccionado
// Abrir menú lateral con detalles del pedido seleccionado
function openOrderSidebar(orderId) {
    const order = orders.find(o => o.id === orderId);
    if (!order) return; // Verificar que el pedido existe

    // Mostrar los detalles del pedido
    document.getElementById("orderId").textContent = order.id;
    document.getElementById("orderDate").textContent = order.fecha;
    document.getElementById("orderUser").textContent = order.usuario;

    // Renderizar la tabla de productos dentro del menú lateral
    renderOrderProductsTable(order);

    // Mostrar el sidebar
    document.getElementById("orderSidebar").classList.remove("hidden");
}

// Renderizar tabla de productos dentro del menú lateral
function renderOrderProductsTable(order) {
    const tableBody = document.getElementById("orderProductsTable");
    tableBody.innerHTML = "";  // Limpiar tabla existente

    if (order.productos.length === 0) {
        const row = document.createElement("tr");
        row.innerHTML = "<td colspan='2'>No hay productos en este pedido.</td>";
        tableBody.appendChild(row);
    } else {
        order.productos.forEach((product, index) => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${product}</td>
                <td>
                    <button onclick="removeProductFromCurrentOrder(${order.id}, ${index})">🗑️</button>
                </td>
            `;
            tableBody.appendChild(row);
        });
    }
}

// Cerrar menú lateral
function closeOrderSidebar() {
    document.getElementById("orderSidebar").classList.add("hidden");
}

// Renderizar tabla de productos dentro del menú lateral
function renderOrderProductsTable(order) {
    const tableBody = document.getElementById("orderProductsTable");
    tableBody.innerHTML = "";
    order.productos.forEach((product, index) => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${product}</td>
            <td>
                <button onclick="removeProductFromCurrentOrder(${order.id}, ${index})">🗑️</button>
            </td>
        `;
        tableBody.appendChild(row);
    });
}

// Agregar producto al pedido seleccionado
function addProductToCurrentOrder() {
    const orderId = parseInt(document.getElementById("orderId").textContent);
    const order = orders.find(o => o.id === orderId);

    const productName = prompt("Ingrese el nombre del producto:");
    const product = products.find(p => p.nombre === productName);

    if (order && product) {
        order.productos.push(product.nombre);
        renderOrderProductsTable(order);
        renderOrderStackPanel();
    } else {
        alert("Producto no encontrado.");
    }
}

// Eliminar producto del pedido seleccionado
function removeProductFromCurrentOrder(orderId, productIndex) {
    const order = orders.find(o => o.id === orderId);
    if (order && order.productos.length > productIndex) {
        order.productos.splice(productIndex, 1);
        renderOrderProductsTable(order);
        renderOrderStackPanel();
    }
}