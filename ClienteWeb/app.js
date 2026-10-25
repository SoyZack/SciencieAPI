const apiUrl = 'http://localhost:5112/api/Cientificos';
let cientificoSeleccionadoId = null;

// Almacena el estado para saber si estamos editando
let cientificoActual = null;
let editandoDescubrimientoId = null;
let editandoInvestigacionId = null;

// Función para obtener y mostrar los científicos
async function cargarCientificos() {
    try {
        const respuesta = await fetch(apiUrl);
        if (!respuesta.ok) throw new Error('Error de red al consultar la API');

        const cientificos = await respuesta.json();
        const contenedor = document.getElementById('contenedor-cientificos');
        contenedor.innerHTML = '';

        if (cientificos.length === 0) {
            contenedor.innerHTML = '<p class="text-center">No hay científicos registrados aún.</p>';
            return;
        }

        cientificos.forEach(cientifico => {
            const card = `
                <div class="col-md-4 mb-4">
                    <div class="card h-100 shadow-sm">
                        <div class="card-body">
                            <h5 class="card-title">${cientifico.nombre} ${cientifico.apellido}</h5>
                            <h6 class="card-subtitle mb-2 text-muted">${cientifico.nacionalidad}</h6>
                            <p class="card-text">
                                <strong>Email:</strong> ${cientifico.email} <br>
                                <strong>Nacimiento:</strong> ${cientifico.fechaNaciemiento || cientifico.fechaNacimiento}
                            </p>
                        </div>
                        <div class="card-footer bg-transparent">
                            <button class="btn btn-sm btn-info" onclick="verDetalles(${cientifico.id})">Ver Detalles</button>
                            <button class="btn btn-sm btn-danger" onclick="eliminarCientifico(${cientifico.id})">Eliminar</button>
                        </div>
                    </div>
                </div>
            `;
            contenedor.innerHTML += card;
        });

    } catch (error) {
        console.error('Hubo un problema con la petición Fetch:', error);
        document.getElementById('contenedor-cientificos').innerHTML =
            '<p class="text-danger text-center">Error al cargar los datos. Verifica que la API esté encendida.</p>';
    }
}

document.addEventListener('DOMContentLoaded', cargarCientificos);

// Función para guardar un nuevo científico con validaciones
async function guardarCientifico() {
    const nombre = document.getElementById('nombre').value.trim();
    const apellido = document.getElementById('apellido').value.trim();
    const nacionalidad = document.getElementById('nacionalidad').value.trim();
    const email = document.getElementById('email').value.trim();
    const inputFecha = document.getElementById('fechaNacimiento');
    const fechaNacimiento = inputFecha.value;

    if (inputFecha.validity.badInput) {
        alert("La fecha ingresada no existe en el calendario. Por favor corrígela.");
        return;
    }

    if (!nombre || !apellido || !nacionalidad || !fechaNacimiento || !email) {
        alert("Por favor completa todos los campos antes de guardar.");
        return;
    }

    const regexCorreo = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!regexCorreo.test(email)) {
        alert("El correo electrónico no es válido.");
        return;
    }

    const nuevoCientifico = {
        nombre: nombre,
        apellido: apellido,
        nacionalidad: nacionalidad,
        fechaNaciemiento: fechaNacimiento,
        email: email,
        especialidadIds: []
    };

    try {
        const respuesta = await fetch(apiUrl, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(nuevoCientifico)
        });

        if (respuesta.ok) {
            const modalEl = document.getElementById('modalCrear');
            let modalInst = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
            modalInst.hide();
            document.getElementById('formCrear').reset();
            cargarCientificos();
        } else {
            const error = await respuesta.json();
            alert(`Error desde el servidor: ${error.mensaje}`);
        }
    } catch (error) {
        console.error('Error al guardar:', error);
        alert('Error de conexión con la API.');
    }
}

async function eliminarCientifico(id) {
    if (!confirm('¿Estás seguro de eliminar este registro?')) return;
    try {
        const respuesta = await fetch(`${apiUrl}/${id}`, { method: 'DELETE' });
        if (respuesta.ok) cargarCientificos();
        else alert('Error al eliminar.');
    } catch (error) {
        console.error('Error al eliminar:', error);
    }
}

async function verDetalles(id) {
    try {
        cientificoSeleccionadoId = id;
        const respuesta = await fetch(`${apiUrl}/${id}`);
        if (!respuesta.ok) throw new Error('Error al obtener los detalles');

        cientificoActual = await respuesta.json();

        document.getElementById('detallesNombre').innerText = `${cientificoActual.nombre} ${cientificoActual.apellido}`;

        document.getElementById('detallesEspecialidades').innerHTML = cientificoActual.especialidades.length > 0
            ? cientificoActual.especialidades.map(e => `<li class="d-flex justify-content-between align-items-center border-bottom pb-1 mb-1">${e.nombre} <button class="btn btn-sm btn-danger py-0 px-2" onclick="removerEspecialidad(${e.id})">X</button></li>`).join('')
            : '<li class="text-muted">Ningún registro</li>';

        // Botón de Editar para Descubrimientos
        document.getElementById('detallesDescubrimientos').innerHTML = cientificoActual.descubrimientos.length > 0
            ? cientificoActual.descubrimientos.map(d => `
                <li class="d-flex justify-content-between align-items-center border-bottom pb-1 mb-1">
                    <div>${d.titulo} <br><small class="text-muted">${d.lugar}</small></div> 
                    <div class="text-nowrap">
                        <button class="btn btn-sm btn-outline-secondary py-0 px-2 me-1" onclick="prepararEdicionDescubrimiento(${d.id})">✎</button>
                        <button class="btn btn-sm btn-danger py-0 px-2" onclick="eliminarDescubrimiento(${d.id})">X</button>
                    </div>
                </li>`).join('')
            : '<li class="text-muted">Ningún registro</li>';

        // Botón de Editar para Investigaciones
        document.getElementById('detallesInvestigaciones').innerHTML = cientificoActual.investigaciones.length > 0
            ? cientificoActual.investigaciones.map(i => `
                <li class="d-flex justify-content-between align-items-center border-bottom pb-1 mb-1">
                    <div>${i.titulo} <br><small><strong>${i.estado}</strong></small></div> 
                    <div class="text-nowrap">
                        <button class="btn btn-sm btn-outline-secondary py-0 px-2 me-1" onclick="prepararEdicionInvestigacion(${i.id})">✎</button>
                        <button class="btn btn-sm btn-danger py-0 px-2" onclick="eliminarInvestigacion(${i.id})">X</button>
                    </div>
                </li>`).join('')
            : '<li class="text-muted">Ningún registro</li>';

        const modal = new bootstrap.Modal(document.getElementById('modalDetalles'));
        modal.show();
    } catch (error) {
        console.error(error);
        alert('Error al cargar los detalles.');
    }
}


// --- LÓGICA PARA DESCUBRIMIENTOS (CREATE Y UPDATE) ---
function abrirModalDescubrimiento() {
    editandoDescubrimientoId = null;
    document.getElementById('formDescubrimiento').reset();
    bootstrap.Modal.getInstance(document.getElementById('modalDetalles')).hide();
    new bootstrap.Modal(document.getElementById('modalDescubrimiento')).show();
}

function prepararEdicionDescubrimiento(id) {
    // Activamos el modo edición
    editandoDescubrimientoId = id; 
    const desc = cientificoActual.descubrimientos.find(d => d.id === id);

    // Llenamos el formulario con los datos extraídos
    document.getElementById('tituloDesc').value = desc.titulo;
    document.getElementById('lugarDesc').value = desc.lugar;
    document.getElementById('fechaDesc').value = desc.fechaDescubrimiento.split('T')[0];
    document.getElementById('descripcionDesc').value = desc.descripcion;

    bootstrap.Modal.getInstance(document.getElementById('modalDetalles')).hide();
    new bootstrap.Modal(document.getElementById('modalDescubrimiento')).show();
}

async function guardarDescubrimiento() {
    const titulo = document.getElementById('tituloDesc').value.trim();
    const lugar = document.getElementById('lugarDesc').value.trim();
    const fecha = document.getElementById('fechaDesc').value;
    const descripcion = document.getElementById('descripcionDesc').value.trim();

    if (!titulo || !lugar || !fecha || !descripcion) {
        alert("Completa todos los campos del descubrimiento.");
        return;
    }

    const payload = {
        titulo: titulo,
        descripcion: descripcion,
        fechaDescubrimiento: fecha,
        lugar: lugar,
        cientificoId: cientificoSeleccionadoId
    };

    if (editandoDescubrimientoId) payload.id = editandoDescubrimientoId;

    // Si estamos editando, usamos la URL con el ID y el método PUT
    const url = editandoDescubrimientoId
        ? `http://localhost:5112/api/Descubrimientos/${editandoDescubrimientoId}`
        : 'http://localhost:5112/api/Descubrimientos';
    const metodo = editandoDescubrimientoId ? 'PUT' : 'POST';

    try {
        const respuesta = await fetch(url, {
            method: metodo,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        if (respuesta.ok) {
            bootstrap.Modal.getInstance(document.getElementById('modalDescubrimiento')).hide();
            verDetalles(cientificoSeleccionadoId);
        } else {
            alert('Error al guardar el descubrimiento.');
        }
    } catch (error) {
        console.error(error);
        alert('Error de conexión.');
    }
}


// --- LÓGICA PARA INVESTIGACIONES (CREATE Y UPDATE) ---
function abrirModalInvestigacion() {
    editandoInvestigacionId = null;
    document.getElementById('formInvestigacion').reset();
    bootstrap.Modal.getInstance(document.getElementById('modalDetalles')).hide();
    new bootstrap.Modal(document.getElementById('modalInvestigacion')).show();
}

function prepararEdicionInvestigacion(id) {
    editandoInvestigacionId = id;
    const inv = cientificoActual.investigaciones.find(i => i.id === id);

    document.getElementById('tituloInv').value = inv.titulo;
    document.getElementById('areaInv').value = inv.areaEstudio;
    document.getElementById('inicioInv').value = inv.fechaInicio.split('T')[0];
    document.getElementById('finInv').value = inv.fechaFin ? inv.fechaFin.split('T')[0] : '';
    document.getElementById('estadoInv').value = inv.estado;

    bootstrap.Modal.getInstance(document.getElementById('modalDetalles')).hide();
    new bootstrap.Modal(document.getElementById('modalInvestigacion')).show();
}

async function guardarInvestigacion() {
    const titulo = document.getElementById('tituloInv').value.trim();
    const areaEstudio = document.getElementById('areaInv').value.trim();
    const fechaInicio = document.getElementById('inicioInv').value;
    const fechaFin = document.getElementById('finInv').value;
    const estado = document.getElementById('estadoInv').value;

    if (!titulo || !areaEstudio || !fechaInicio || !estado) {
        alert("Completa todos los campos obligatorios.");
        return;
    }

    const payload = {
        titulo: titulo,
        areaEstudio: areaEstudio,
        fechaInicio: fechaInicio,
        fechaFin: fechaFin ? fechaFin : null,
        estado: estado,
        cientificoId: cientificoSeleccionadoId
    };

    if (editandoInvestigacionId) payload.id = editandoInvestigacionId;

    const url = editandoInvestigacionId
        ? `http://localhost:5112/api/Investigaciones/${editandoInvestigacionId}`
        : 'http://localhost:5112/api/Investigaciones';
    const metodo = editandoInvestigacionId ? 'PUT' : 'POST';

    try {
        const respuesta = await fetch(url, {
            method: metodo,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        if (respuesta.ok) {
            bootstrap.Modal.getInstance(document.getElementById('modalInvestigacion')).hide();
            verDetalles(cientificoSeleccionadoId);
        } else {
            alert('Error al guardar la investigación.');
        }
    } catch (error) {
        console.error(error);
    }
}


// --- LÓGICA PARA ESPECIALIDADES ---
async function abrirModalEspecialidad() {
    bootstrap.Modal.getInstance(document.getElementById('modalDetalles')).hide();
    const modalEspecialidad = new bootstrap.Modal(document.getElementById('modalEspecialidad'));

    try {
        const respuesta = await fetch('http://localhost:5112/api/Especialidades');
        const especialidades = await respuesta.json();
        const select = document.getElementById('selectEspecialidad');
        select.innerHTML = '';

        especialidades.forEach(esp => {
            select.innerHTML += `<option value="${esp.id}">${esp.nombre}</option>`;
        });
    } catch (error) {
        console.error('Error cargando especialidades:', error);
    }

    modalEspecialidad.show();
}

async function asignarEspecialidad() {
    const opcionesSeleccionadas = Array.from(document.getElementById('selectEspecialidad').selectedOptions);
    if (opcionesSeleccionadas.length === 0) {
        alert("Por favor selecciona al menos una especialidad.");
        return;
    }
    try {
        const promesas = opcionesSeleccionadas.map(opcion => {
            return fetch(`${apiUrl}/${cientificoSeleccionadoId}/especialidades/${opcion.value}`, {
                method: 'POST'
            });
        });
        await Promise.all(promesas);
        bootstrap.Modal.getInstance(document.getElementById('modalEspecialidad')).hide();
        verDetalles(cientificoSeleccionadoId);
    } catch (error) {
        console.error(error);
        alert("Ocurrió un error al asignar las especialidades.");
    }
}


// --- FUNCIONES PARA ELIMINAR REGISTROS INDIVIDUALES ---
async function removerEspecialidad(especialidadId) {
    if (!confirm("¿Remover esta especialidad del científico?")) return;
    await fetch(`${apiUrl}/${cientificoSeleccionadoId}/especialidades/${especialidadId}`, { method: 'DELETE' });
    verDetalles(cientificoSeleccionadoId);
}

async function eliminarDescubrimiento(id) {
    if (!confirm("¿Eliminar este descubrimiento?")) return;
    await fetch(`http://localhost:5112/api/Descubrimientos/${id}`, { method: 'DELETE' });
    verDetalles(cientificoSeleccionadoId);
}

async function eliminarInvestigacion(id) {
    if (!confirm("¿Eliminar esta investigación?")) return;
    await fetch(`http://localhost:5112/api/Investigaciones/${id}`, { method: 'DELETE' });
    verDetalles(cientificoSeleccionadoId);
}


// --- FUNCIÓN DE BÚSQUEDA EN TIEMPO REAL ---
function filtrarCientificos() {
    const textoBusqueda = document.getElementById('buscador').value.toLowerCase();
    const tarjetas = document.querySelectorAll('#contenedor-cientificos .col-md-4');

    tarjetas.forEach(tarjeta => {
        // Busca el título dentro de la tarjeta
        const nombreCientifico = tarjeta.querySelector('.card-title').innerText.toLowerCase();
        if (nombreCientifico.includes(textoBusqueda)) {
            tarjeta.style.display = '';
        } else {
            tarjeta.style.display = 'none';
        }
    });
}