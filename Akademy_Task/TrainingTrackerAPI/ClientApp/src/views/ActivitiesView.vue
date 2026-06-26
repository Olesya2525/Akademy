<template>
    <div>
        <h1>My Activities</h1>

        <div v-if="storedUserId">
            <div style="margin:20px 0; padding:15px; background:#f5f5f5; border-radius:8px;">
                <h3>Add Activity</h3>
                <input v-model.number="newActivity.exerciseId" placeholder="Exercise ID" type="number" />
                <input type="date" v-model="newActivity.activityDate" />
                <input v-model.number="newActivity.durationMinutes" placeholder="Minutes" type="number" />
                <input v-model="newActivity.note" placeholder="Note" />
                <button @click="createActivity">Add</button>
                <p v-if="activityError" style="color:red;">{{ activityError }}</p>
                <p v-if="activitySuccess" style="color:green;">{{ activitySuccess }}</p>
            </div>

            <ul>
                <li v-for="a in activities" :key="a.activityId">
                    <span v-if="editActivityId !== a.activityId">
                        {{ formatDate(a.activityDate) }} — {{ a.durationMinutes }} min
                        <span v-if="a.exercise">({{ a.exercise.name }})</span>
                        <span v-if="a.note">— {{ a.note }}</span>
                        <button @click="startEditActivity(a)">Edit</button>
                        <button @click="deleteActivity(a.activityId)" style="background:#e74c3c; color:white;">Delete</button>
                    </span>
                    <span v-else>
                        <input v-model.number="editActivityData.exerciseId" placeholder="Exercise ID" type="number" />
                        <input type="date" v-model="editActivityData.activityDate" />
                        <input v-model.number="editActivityData.durationMinutes" placeholder="Minutes" type="number" />
                        <input v-model="editActivityData.note" placeholder="Note" />
                        <button @click="saveEditActivity">Save</button>
                        <button @click="cancelEditActivity">Cancel</button>
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
        name: 'ActivitiesView',
        data() {
            return {
                storedUserId: '',
                activities: [],
                newActivity: {
                    exerciseId: null,
                    activityDate: '',
                    durationMinutes: 0,
                    note: ''
                },
                activityError: '',
                activitySuccess: '',
                editActivityId: null,
                editActivityData: {
                    exerciseId: null,
                    activityDate: '',
                    durationMinutes: 0,
                    note: ''
                }
            }
        },
        mounted() {
            this.storedUserId = localStorage.getItem('userId') || ''
            if (this.storedUserId) this.loadActivities()
        },
        methods: {
            formatDate(dateString) {
                if (!dateString) return ''
                const date = new Date(dateString)
                return date.toLocaleDateString('ru-RU')
            },
            async loadActivities() {
                try {
                    const res = await api.getActivitiesByUser(this.storedUserId)
                    this.activities = res.data
                    this.activityError = ''
                } catch (err) {
                    this.activityError = 'Error loading activities'
                    console.error(err)
                }
            },
            async createActivity() {
                this.activityError = ''
                this.activitySuccess = ''

                if (!this.newActivity.exerciseId || !this.newActivity.activityDate || !this.newActivity.durationMinutes) {
                    this.activityError = 'Please fill in all fields'
                    return
                }

                try {
                    await api.createActivity({
                        ...this.newActivity,
                        userId: this.storedUserId
                    })
                    this.newActivity = { exerciseId: null, activityDate: '', durationMinutes: 0, note: '' }
                    this.activitySuccess = 'Activity created successfully'
                    this.loadActivities()
                } catch (err) {
                    this.activityError = 'Error creating activity'
                    console.error(err)
                }
            },
            startEditActivity(activity) {
                this.editActivityId = activity.activityId
                this.editActivityData = { ...activity }
            },
            async saveEditActivity() {
                this.activityError = ''
                this.activitySuccess = ''

                if (!this.editActivityData.exerciseId || !this.editActivityData.activityDate || !this.editActivityData.durationMinutes) {
                    this.activityError = 'Please fill in all fields'
                    return
                }

                try {
                    await api.updateActivity(this.editActivityId, {
                        ...this.editActivityData,
                        userId: this.storedUserId
                    })
                    this.editActivityId = null
                    this.editActivityData = { exerciseId: null, activityDate: '', durationMinutes: 0, note: '' }
                    this.activitySuccess = 'Activity updated successfully'
                    this.loadActivities()
                } catch (err) {
                    this.activityError = 'Error updating activity'
                    console.error(err)
                }
            },
            cancelEditActivity() {
                this.editActivityId = null
                this.editActivityData = { exerciseId: null, activityDate: '', durationMinutes: 0, note: '' }
            },
            async deleteActivity(id) {
                this.activityError = ''
                this.activitySuccess = ''

                if (!confirm('Delete this activity?')) return

                try {
                    await api.deleteActivity(id)
                    this.activitySuccess = 'Activity deleted successfully'
                    this.loadActivities()
                } catch (err) {
                    this.activityError = 'Error deleting activity'
                    console.error(err)
                }
            }
        }
    }
</script>