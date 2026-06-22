<template>
    <div>
        <h1>My Programs</h1>

        <div v-if="storedUserId">
            <div style="margin:20px 0; padding:15px; background:#f5f5f5; border-radius:8px;">
                <h3>Add Program</h3>
                <input v-model="newProgram.name" placeholder="Name" />
                <input v-model="newProgram.type" placeholder="Type (Strength/Cardio)" />
                <button @click="createProgram">Add</button>
                <p v-if="programError" style="color:red;">{{ programError }}</p>
                <p v-if="programSuccess" style="color:green;">{{ programSuccess }}</p>
            </div>

            <ul>
                <li v-for="p in programs" :key="p.programId">
                    <strong>ID: {{ p.programId }}</strong> — <strong>{{ p.name }}</strong> ({{ p.type }})
                    - {{ p.isActive ? '🟢 Active' : '🔴 Inactive' }}
                    <button @click="deleteProgram(p.programId)" style="background:#e74c3c; color:white;">Delete</button>
                </li>
            </ul>
        </div>
        <p v-else>Please login first</p>
    </div>
</template>

<script>
    import { api } from '@/api'

    export default {
        name: 'ProgramsView',
        data() {
            return {
                storedUserId: '',
                programs: [],
                newProgram: { name: '', type: '', isActive: true },
                programError: '',
                programSuccess: ''
            }
        },
        mounted() {
            this.storedUserId = localStorage.getItem('userId') || ''
            if (this.storedUserId) this.loadPrograms()
        },
        methods: {
            async loadPrograms() {
                try {
                    const res = await api.getProgramsByUser(this.storedUserId)
                    this.programs = res.data
                    this.programError = ''
                } catch (err) {
                    if (err.response) {
                        this.programError = err.response.data || 'Error loading programs'
                    } else {
                        this.programError = 'Network error - check if server is running'
                    }
                    console.error(err)
                }
            },
            async createProgram() {
                this.programError = ''
                this.programSuccess = ''

                if (!this.newProgram.name) {
                    this.programError = 'Program name is required'
                    return
                }

                if (!this.newProgram.type) {
                    this.programError = 'Program type is required (Strength, Cardio, or Stretching)'
                    return
                }

                try {
                    await api.createProgram({
                        ...this.newProgram,
                        userId: this.storedUserId
                    })
                    this.newProgram = { name: '', type: '', isActive: true }
                    this.programSuccess = 'Program created successfully'
                    this.loadPrograms()
                } catch (err) {
                    if (err.response) {
                        this.programError = err.response.data || 'Error creating program'
                    } else {
                        this.programError = 'Network error - check if server is running'
                    }
                    console.error(err)
                }
            },
            async deleteProgram(id) {
                this.programError = ''
                this.programSuccess = ''

                if (!confirm('Delete this program?')) return

                try {
                    await api.deleteProgram(id, this.storedUserId)
                    this.programSuccess = 'Program deleted successfully'
                    this.loadPrograms()
                } catch (err) {
                    if (err.response) {
                        this.programError = err.response.data || 'Error deleting program'
                    } else {
                        this.programError = 'Network error - check if server is running'
                    }
                    console.error(err)
                }
            }
        }
    }
</script>