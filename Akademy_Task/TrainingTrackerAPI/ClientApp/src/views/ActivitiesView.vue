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
                    {{ a.activityDate }} - {{ a.durationMinutes }} - {{ a.note }}
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
                activitySuccess: ''
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
                    if (err.response) {
                        this.activityError = err.response.data || 'Error loading activities'
                    } else {
                        this.activityError = 'Network error - check if server is running'
                    }
                    console.error(err)
                }
            },
            async createActivity() {
                this.activityError = ''
                this.activitySuccess = ''

                if (!this.newActivity.exerciseId) {
                    this.activityError = 'Exercise ID is required'
                    return
                }

                if (!this.newActivity.activityDate) {
                    this.activityError = 'Date is required'
                    return
                }

                if (!this.newActivity.durationMinutes || this.newActivity.durationMinutes <= 0) {
                    this.activityError = 'Minutes must be greater than 0'
                    return
                }

                if (this.newActivity.durationMinutes > 1440) {
                    this.activityError = 'Minutes cannot exceed 1440 (24 hours)'
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
                    if (err.response) {
                        this.activityError = err.response.data || 'Error creating activity'
                    } else {
                        this.activityError = 'Network error - check if server is running'
                    }
                    console.error(err)
                }
            }
        }
    }
</script>