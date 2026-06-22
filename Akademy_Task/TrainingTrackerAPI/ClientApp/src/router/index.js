import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: 'home',
            component: HomeView
        },
        {
            path: '/programs',
            name: 'programs',
            component: () => import('../views/ProgramsView.vue')
        },
        {
            path: '/exercises',
            name: 'exercises',
            component: () => import('../views/ExercisesView.vue')
        },
        {
            path: '/activities',
            name: 'activities',
            component: () => import('../views/ActivitiesView.vue')
        },
        {
            path: '/report',
            name: 'report',
            component: () => import('../views/ReportView.vue')
        }
    ]
})

export default router