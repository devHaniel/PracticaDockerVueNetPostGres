```vue
<script setup lang="ts">
import { onMounted, ref } from 'vue'

interface Product {
  id: number
  name: string
  price: number
}

const products = ref<Product[]>([])

const name = ref('')
const price = ref<number | null>(null)
const loading = ref(false)

const API_URL = import.meta.env.VITE_API_URL

const getProducts = async () => {
  const response = await fetch(`${API_URL}/api/products`)

  if (!response.ok) {
    throw new Error('Error al obtener productos')
  }

  products.value = await response.json()
}

const createProduct = async () => {
  if (!name.value || price.value === null) return

  loading.value = true

  try {
    const response = await fetch(`${API_URL}/api/products`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        name: name.value,
        price: price.value
      })
    })

    if (!response.ok) {
      alert('Error al crear producto')
      return
    }

    name.value = ''
    price.value = null

    await getProducts()
  } finally {
    loading.value = false
  }
}

onMounted(getProducts)
</script>

<template>
  <main class="container">
    <h1>Productos</h1>

    <!-- Tabla -->
    <section class="table-section">
      <h2>Lista de productos</h2>

      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Nombre</th>
            <th>Precio</th>
          </tr>
        </thead>

        <tbody>
          <tr v-for="product in products" :key="product.id">
            <td>{{ product.id }}</td>
            <td>{{ product.name }}</td>
            <td>L. {{ product.price.toFixed(2) }}</td>
          </tr>

          <tr v-if="products.length === 0">
            <td colspan="3">No hay productos.</td>
          </tr>
        </tbody>
      </table>
    </section>

    <!-- Crear producto -->
    <section class="create-section">
      <h2>Crear producto</h2>

      <form @submit.prevent="createProduct">
        <div>
          <label>Nombre</label>
          <input
            v-model="name"
            type="text"
            placeholder="Nombre del producto"
            required
          />
        </div>

        <div>
          <label>Precio</label>
          <input
            v-model.number="price"
            type="number"
            step="0.01"
            min="0"
            placeholder="0.00"
            required
          />
        </div>

        <button type="submit" :disabled="loading">
          {{ loading ? 'Creando...' : 'Crear producto' }}
        </button>
      </form>
    </section>
  </main>
</template>

<style scoped>
.container {
  max-width: 1000px;
  margin: 40px auto;
  padding: 0 20px;
  font-family: Arial, sans-serif;
}

h1 {
  margin-bottom: 30px;
}

h2 {
  margin-bottom: 15px;
}

.table-section {
  margin-bottom: 40px;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  padding: 12px;
  border: 1px solid #ddd;
  text-align: left;
}

th {
  background: #f5f5f5;
}

.create-section {
  max-width: 400px;
}

form {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

input {
  width: 100%;
  padding: 10px;
  box-sizing: border-box;
}

button {
  padding: 10px;
  cursor: pointer;
}
</style>
