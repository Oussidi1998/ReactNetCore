import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Login } from "./components/Login";

import './custom.css'

export default class App extends Component {

  render () {
    return (
      <Layout>
            <Route exact path='/' component={Home} />
            <Route exact path='/home' component={Home} />
        <Route path='/login' component={Login} />
      </Layout>
    );
  }
}
