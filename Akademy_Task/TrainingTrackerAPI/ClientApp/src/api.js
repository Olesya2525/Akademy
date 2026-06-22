import axios from 'axios'

const API_URL = 'https://localhost:7186/api'

export const api = {
    getUsers: () => axios.get(`${API_URL}/Users`),
    createUser: (user) => axios.post(`${API_URL}/Users`, user),

    getProgramsByUser: (userId) => axios.get(`${API_URL}/Programs/byuser/${userId}`),
    createProgram: (program) => axios.post(`${API_URL}/Programs`, program),
    updateProgram: (id, program) => axios.put(`${API_URL}/Programs/${id}`, program),
    deleteProgram: (id, userId) => axios.delete(`${API_URL}/Programs/${id}?userId=${userId}`),

    getExercisesByUser: (userId) => axios.get(`${API_URL}/Exercises/byuser/${userId}`),
    createExercise: (exercise, userId) => axios.post(`${API_URL}/Exercises?userId=${userId}`, exercise),
    deleteExercise: (id, userId) => axios.delete(`${API_URL}/Exercises/${id}?userId=${userId}`),

    getActivitiesByUser: (userId) => axios.get(`${API_URL}/Activities/byuser/${userId}`),
    createActivity: (activity) => axios.post(`${API_URL}/Activities`, activity),
    getDailyReport: (date, userId) => axios.get(`${API_URL}/Activities/daily-report?date=${date}&userId=${userId}`),
    getDailyReportByUrl: (url) => axios.get(`${API_URL}${url}`)
}
