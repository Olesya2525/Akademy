<template>
    <div>
        <h1>Daily Report</h1>

        <div v-if="storedUserId">
            <div style="margin:20px 0; padding:15px; background:#f5f5f5; border-radius:8px;">
                <h3>Select period</h3>

                <div style="display:flex; gap:10px; flex-wrap:wrap; align-items:center;">
                    <!-- Выбор периода -->
                    <select v-model="period">
                        <option value="day">Day</option>
                        <option value="month">Month</option>
                        <option value="all">All time</option>
                    </select>

                    <!-- Для дня -->
                    <input v-if="period === 'day'" type="date" v-model="date" />

                    <!-- Для месяца -->
                    <div v-if="period === 'month'" style="display:flex; gap:5px;">
                        <input type="number" v-model="monthYear" placeholder="2026" style="width:80px;" />
                        <select v-model="monthNumber">
                            <option v-for="m in 12" :key="m" :value="m">{{ m }}</option>
                        </select>
                    </div>

                    <button @click="loadReport">Get Report</button>
                </div>

                <p v-if="reportError" style="color:red;">{{ reportError }}</p>
            </div>

            <div v-if="report">
                <!-- Стикер (только для дня) -->
                <div v-if="period === 'day'" class="sticker" :style="{ backgroundColor: report.sticker }">
                    <strong>Sticker:</strong> {{ report.stickerMessage }}
                    <br>
                    <strong>Total minutes:</strong> {{ report.totalMinutes }}
                </div>

                <!-- Для месяца и всё время -->
                <div v-if="period === 'month' || period === 'all'" style="padding:10px; background:#e8f5e9; border-radius:8px; margin-bottom:15px;">
                    <strong>Total minutes:</strong> {{ report.totalMinutes }}
                </div>

                <h3>Activities</h3>
                <ul>
                    <li v-for="a in report.activities" :key="a.activityId">
                        {{ a.activityDate }} - {{ a.durationMinutes }} min
                        <span v-if="a.exercise">({{ a.exercise.name }})</span>
                        <span v-if="a.note">- {{ a.note }}</span>
                    </li>
                </ul>
            </div>
        </div>
        <p v-else>Please login first</p>
    </div>
</template>

<script>
    import { api } from '@/api'

    export default {
        name: 'ReportView',
        data() {
            return {
                storedUserId: '',
                period: 'day',
                date: '',
                monthYear: new Date().getFullYear(),
                monthNumber: new Date().getMonth() + 1,
                report: null,
                reportError: ''
            }
        },
        mounted() {
            this.storedUserId = localStorage.getItem('userId') || ''
            const today = new Date()
            this.date = today.toISOString().split('T')[0]
            this.monthYear = today.getFullYear()
            this.monthNumber = today.getMonth() + 1
        },
        methods: {
            formatDate(dateString) {
                if (!dateString) return ''
                const date = new Date(dateString)
                return date.toLocaleDateString('ru-RU')
            },
            async loadReport() {
                this.reportError = ''
                this.report = null

                try {
                    let url = ''

                    if (this.period === 'day') {
                        if (!this.date) {
                            this.reportError = 'Please select a date'
                            return
                        }
                        url = `/Activities/daily-report?date=${this.date}`
                    }
                    else if (this.period === 'month') {
                        if (!this.monthYear || !this.monthNumber) {
                            this.reportError = 'Please select year and month'
                            return
                        }
                        url = `/Activities/bymonth?year=${this.monthYear}&month=${this.monthNumber}&userId=${this.storedUserId}`
                    }
                    else {
                        url = `/Activities/byuser/${this.storedUserId}`
                    }

                    const res = await api.getDailyReportByUrl(url)

                    if (this.period === 'day') {
                        this.report = res.data
                    } else {
                        this.report = {
                            totalMinutes: res.data.reduce((sum, a) => sum + a.durationMinutes, 0),
                            activities: res.data
                        }
                    }
                } catch (err) {
                    if (err.response) {
                        this.reportError = err.response.data || 'Error getting report'
                    } else {
                        this.reportError = 'Network error - check if server is running'
                    }
                    console.error(err)
                }
            }
        }
    }
</script>

<style scoped>
    .sticker {
        padding: 10px;
        border-radius: 5px;
        margin: 10px 0;
    }
</style>