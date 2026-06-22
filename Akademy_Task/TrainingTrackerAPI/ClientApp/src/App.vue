<template>
    <div id="app">
        <nav>
            <router-link to="/">Home</router-link> |
            <router-link to="/programs">Programs</router-link> |
            <router-link to="/exercises">Exercises</router-link> |
            <router-link to="/activities">Activities</router-link> |
            <router-link to="/report">Report</router-link>
        </nav>
        <router-view />
    </div>
</template>

<script>
    export default {
        name: 'App'
    }
</script>

<style>
    #app {
        font-family: Arial, sans-serif;
        padding: 20px;
        max-width: 1200px;
        margin: 0 auto;
    }

    nav {
        margin-bottom: 20px;
        padding: 10px;
        background: #f5f5f5;
        border-radius: 8px;
    }

        nav a {
            margin-right: 15px;
            color: #2c3e50;
            text-decoration: none;
            font-weight: 500;
        }

            nav a.router-link-active {
                color: #42b983;
                font-weight: bold;
            }

    button {
        background: #42b983;
        color: white;
        border: none;
        padding: 8px 16px;
        border-radius: 4px;
        cursor: pointer;
        margin: 5px;
    }

        button:hover {
            background: #3aa876;
        }

    input {
        padding: 8px;
        margin: 5px;
        border: 1px solid #ddd;
        border-radius: 4px;
    }

    ul {
        list-style: none;
        padding: 0;
    }

    li {
        padding: 8px;
        margin: 5px 0;
        background: #f9f9f9;
        border-radius: 4px;
    }

    .sticker {
        padding: 10px;
        border-radius: 5px;
        margin: 10px 0;
    }
</style>