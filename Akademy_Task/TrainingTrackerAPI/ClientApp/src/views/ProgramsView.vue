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
                    <span v-if="editProgramId !== p.programId">
                        <strong>ID: {{ p.programId }}</strong> — <strong>{{ p.name }}</strong> ({{ p.type }})
                        - {{ p.isActive ? '🟢 Active' : '🔴 Inactive' }}
                        <button @click="startEditProgram(p)">Edit</button>
                        <button @click="deleteProgram(p.programId)" style="background:#e74c3c; color:white;">Delete</button>
                    </span>
                    <span v-else>
                        <input v-model="editProgramData.name" placeholder="Name" />
                        <input v-model="editProgramData.type" placeholder="Type" />
                        <label>
                            <input type="checkbox" v-model="editProgramData.isActive" /> Active
                        </label>
                        <button @click="saveEditProgram">Save</button>
                        <button @click="cancelEditProgram">Cancel</button>
                    </span>
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
                programSuccess: '',
                editProgramId: null,
                editProgramData: { name: '', type: '', isActive: true }
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
                    this.programError = 'Error loading programs'
                    console.error(err)
                }
            },
            async createProgram() {
                this.programError = ''
                this.programSuccess = ''

                if (!this.newProgram.name || !this.newProgram.type) {
                    this.programError = 'Please fill in all fields'
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
                    this.programError = 'Error creating program'
                    console.error(err)
                }
            },
            startEditProgram(program) {
                this.editProgramId = program.programId
                this.editProgramData = { ...program }
            },
            async saveEditProgram() {
                this.programError = ''
                this.programSuccess = ''

                if (!this.editProgramData.name || !this.editProgramData.type) {
                    this.programError = 'Please fill in all fields'
                    return
                }

                try {
                    await api.updateProgram(this.editProgramId, {
                        ...this.editProgramData,
                        userId: this.storedUserId
                    })
                    this.editProgramId = null
                    this.editProgramData = { name: '', type: '', isActive: true }
                    this.programSuccess = 'Program updated successfully'
                    this.loadPrograms()
                } catch (err) {
                    this.programError = 'Error updating program'
                    console.error(err)
                }
            },
            cancelEditProgram() {
                this.editProgramId = null
                this.editProgramData = { name: '', type: '', isActive: true }
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
                    this.programError = 'Error deleting program'
                    console.error(err)
                }
            }
        }
    }
</script>