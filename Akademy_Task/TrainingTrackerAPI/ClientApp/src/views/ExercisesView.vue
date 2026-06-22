<template>
    <div>
        <h1>My Exercises</h1>

        <div v-if="storedUserId">
            <div style="margin:20px 0; padding:15px; background:#f5f5f5; border-radius:8px;">
                <h3>Add Exercise</h3>
                <input v-model="newExercise.name" placeholder="Name" />
                <input v-model.number="newExercise.programId" placeholder="Program ID" type="number" />
                <button @click="createExercise">Add</button>
                <p v-if="exerciseError" style="color:red;">{{ exerciseError }}</p>
                <p v-if="exerciseSuccess" style="color:green;">{{ exerciseSuccess }}</p>
            </div>

            <ul>
                <li v-for="e in exercises" :key="e.exerciseId">
                    <strong>ID: {{ e.exerciseId }}</strong> — <strong>{{ e.name }}</strong>
                    - Program: {{ e.program?.name || e.programId }}
                    - {{ e.isActive ? '🟢 Active' : '🔴 Inactive' }}
                    <button @click="deleteExercise(e.exerciseId)" style="background:#e74c3c; color:white;">Delete</button>
                </li>
            </ul>
        </div>
        <p v-else>Please login first</p>
    </div>
</template>

<script>
    import { api } from '@/api'

    export default {
        name: 'ExercisesView',
        data() {
            return {
                storedUserId: '',
                exercises: [],
                newExercise: { name: '', programId: null, isActive: true },
                exerciseError: '',
                exerciseSuccess: ''
            }
        },
        mounted() {
            this.storedUserId = localStorage.getItem('userId') || ''
            if (this.storedUserId) this.loadExercises()
        },
        methods: {
            async loadExercises() {
                try {
                    const res = await api.getExercisesByUser(this.storedUserId)
                    this.exercises = res.data
                    this.exerciseError = ''
                } catch (err) {
                    if (err.response) {
                        this.exerciseError = err.response.data || 'Error loading exercises'
                    } else {
                        this.exerciseError = 'Network error - check if server is running'
                    }
                    console.error(err)
                }
            },
            async createExercise() {
                this.exerciseError = ''
                this.exerciseSuccess = ''

                if (!this.newExercise.name) {
                    this.exerciseError = 'Exercise name is required'
                    return
                }

                if (!this.newExercise.programId) {
                    this.exerciseError = 'Program ID is required'
                    return
                }

                try {
                    await api.createExercise(this.newExercise, this.storedUserId)
                    this.newExercise = { name: '', programId: null, isActive: true }
                    this.exerciseSuccess = 'Exercise created successfully'
                    this.loadExercises()
                } catch (err) {
                    if (err.response) {
                        this.exerciseError = err.response.data || 'Error creating exercise'
                    } else {
                        this.exerciseError = 'Network error - check if server is running'
                    }
                    console.error(err)
                }
            },
            async deleteExercise(id) {
                this.exerciseError = ''
                this.exerciseSuccess = ''

                if (!confirm('Delete this exercise?')) return

                try {
                    await api.deleteExercise(id, this.storedUserId)
                    this.exerciseSuccess = 'Exercise deleted successfully'
                    this.loadExercises()
                } catch (err) {
                    if (err.response) {
                        this.exerciseError = err.response.data || 'Error deleting exercise'
                    } else {
                        this.exerciseError = 'Network error - check if server is running'
                    }
                    console.error(err)
                }
            }
        }
    }
</script>