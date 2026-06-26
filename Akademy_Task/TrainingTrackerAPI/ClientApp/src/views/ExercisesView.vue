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
                    <span v-if="editExerciseId !== e.exerciseId">
                        <strong>ID: {{ e.exerciseId }}</strong> — <strong>{{ e.name }}</strong>
                        - Program: {{ e.program?.name || e.programId }}
                        - {{ e.isActive ? '🟢 Active' : '🔴 Inactive' }}
                        <button @click="startEditExercise(e)">Edit</button>
                        <button @click="deleteExercise(e.exerciseId)" style="background:#e74c3c; color:white;">Delete</button>
                    </span>
                    <span v-else>
                        <input v-model="editExerciseData.name" placeholder="Name" />
                        <input v-model.number="editExerciseData.programId" placeholder="Program ID" type="number" />
                        <label>
                            <input type="checkbox" v-model="editExerciseData.isActive" /> Active
                        </label>
                        <button @click="saveEditExercise">Save</button>
                        <button @click="cancelEditExercise">Cancel</button>
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
        name: 'ExercisesView',
        data() {
            return {
                storedUserId: '',
                exercises: [],
                newExercise: { name: '', programId: null, isActive: true },
                exerciseError: '',
                exerciseSuccess: '',
                editExerciseId: null,
                editExerciseData: { name: '', programId: null, isActive: true }
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
                    this.exerciseError = 'Error loading exercises'
                    console.error(err)
                }
            },
            async createExercise() {
                this.exerciseError = ''
                this.exerciseSuccess = ''

                if (!this.newExercise.name || !this.newExercise.programId) {
                    this.exerciseError = 'Please fill in all fields'
                    return
                }

                try {
                    await api.createExercise(this.newExercise, this.storedUserId)
                    this.newExercise = { name: '', programId: null, isActive: true }
                    this.exerciseSuccess = 'Exercise created successfully'
                    this.loadExercises()
                } catch (err) {
                    this.exerciseError = 'Error creating exercise'
                    console.error(err)
                }
            },
            startEditExercise(exercise) {
                this.editExerciseId = exercise.exerciseId
                this.editExerciseData = { ...exercise }
            },
            async saveEditExercise() {
                this.exerciseError = ''
                this.exerciseSuccess = ''

                if (!this.editExerciseData.name || !this.editExerciseData.programId) {
                    this.exerciseError = 'Please fill in all fields'
                    return
                }

                try {
                    await api.updateExercise(this.editExerciseId, {
                        ...this.editExerciseData,
                        userId: this.storedUserId
                    })
                    this.editExerciseId = null
                    this.editExerciseData = { name: '', programId: null, isActive: true }
                    this.exerciseSuccess = 'Exercise updated successfully'
                    this.loadExercises()
                } catch (err) {
                    this.exerciseError = 'Error updating exercise'
                    console.error(err)
                }
            },
            cancelEditExercise() {
                this.editExerciseId = null
                this.editExerciseData = { name: '', programId: null, isActive: true }
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
                    this.exerciseError = 'Error deleting exercise'
                    console.error(err)
                }
            }
        }
    }
</script>