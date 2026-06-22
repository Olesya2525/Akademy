<template>
    <div>
        <h1>Training Tracker</h1>

        <div v-if="!isLoggedIn">
            <div style="margin:20px 0; padding:20px; background:#f5f5f5; border-radius:8px; max-width:400px;">
                <h3>Login or Register</h3>
                <input v-model="username" placeholder="Enter your username" size="30" />
                <br />
                <button @click="loginOrRegister">Login / Register</button>
                <p v-if="loginError" style="color:red;">{{ loginError }}</p>
                <p v-if="loginSuccess" style="color:green;">{{ loginSuccess }}</p>
            </div>
        </div>

        <div v-if="isLoggedIn" style="margin-top:20px; padding:15px; background:#e8f5e9; border-radius:8px;">
            <p><strong>Welcome, {{ userFullName }}!</strong> ({{ storedUserId }})</p>
            <button @click="logout" style="background:#e74c3c; color:white;">Logout</button>

            <div style="margin-top:20px;">
                <h3>Statistics</h3>
                <button @click="loadStats">Update statistics</button>
                <p v-if="statsError" style="color:red;">{{ statsError }}</p>

                <div v-if="stats">
                    <p><strong>Total minutes:</strong> {{ stats.totalMinutes }}</p>
                    <h4>By program:</h4>
                    <ul>
                        <li v-for="(minutes, program) in stats.minutesByProgram" :key="program">
                            {{ program }}: {{ minutes }} min
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import { api } from '@/api'

    export default {
        name: 'HomeView',
        data() {
            return {
                username: '',
                storedUserId: '',
                userFullName: '',
                isLoggedIn: false,
                loginError: '',
                loginSuccess: '',
                statsError: '',
                stats: null
            }
        },
        mounted() {
            this.storedUserId = localStorage.getItem('userId') || ''
            this.userFullName = localStorage.getItem('userFullName') || ''
            if (this.storedUserId && this.userFullName) {
                this.isLoggedIn = true
                this.loadStats()
            }
        },
        methods: {
            async loginOrRegister() {
                this.loginError = ''
                this.loginSuccess = ''

                if (!this.username.trim()) {
                    this.loginError = 'Please enter a username'
                    return
                }

                try {
                    const usersRes = await api.getUsers()
                    const existingUser = usersRes.data.find(
                        u => u.username.toLowerCase() === this.username.trim().toLowerCase()
                    )

                    if (existingUser) {
                        this.storedUserId = existingUser.userId
                        this.userFullName = existingUser.fullName
                        this.isLoggedIn = true
                        localStorage.setItem('userId', this.storedUserId)
                        localStorage.setItem('userFullName', this.userFullName)
                        this.loginSuccess = `Welcome back, ${this.userFullName}!`
                        this.loadStats()
                    } else {
                        const newUser = {
                            username: this.username.trim(),
                            email: `${this.username.trim().toLowerCase()}@mail.ru`,
                            fullName: this.username.trim(),
                            isActive: true
                        }

                        const createRes = await api.createUser(newUser)
                        this.storedUserId = createRes.data.userId
                        this.userFullName = createRes.data.fullName
                        this.isLoggedIn = true
                        localStorage.setItem('userId', this.storedUserId)
                        localStorage.setItem('userFullName', this.userFullName)
                        this.loginSuccess = `Welcome, ${this.userFullName}! You have been registered`
                        this.loadStats()
                    }

                    this.username = ''

                } catch (err) {
                    if (err.response) {
                        this.loginError = err.response.data || 'Error connecting to server'
                    } else {
                        this.loginError = 'Network error - check if server is running'
                    }
                    console.error(err)
                }
            },

            logout() {
                localStorage.removeItem('userId')
                localStorage.removeItem('userFullName')
                this.isLoggedIn = false
                this.storedUserId = ''
                this.userFullName = ''
                this.stats = null
                this.loginSuccess = ''
                this.loginError = ''
            },

            async loadStats() {
                this.statsError = ''
                this.stats = null

                if (!this.storedUserId) return

                try {
                    const res = await api.getActivitiesByUser(this.storedUserId)
                    const activities = res.data
                    const totalMinutes = activities.reduce((sum, a) => sum + a.durationMinutes, 0)

                    const minutesByProgram = {}
                    activities.forEach(a => {
                        const programName = a.exercise?.program?.name || 'No program'
                        minutesByProgram[programName] = (minutesByProgram[programName] || 0) + a.durationMinutes
                    })

                    this.stats = { totalMinutes, minutesByProgram }
                } catch (err) {
                    if (err.response) {
                        this.statsError = err.response.data || 'Error loading statistics'
                    } else {
                        this.statsError = 'Network error - check if server is running'
                    }
                    console.error(err)
                }
            }
        }
    }
</script>