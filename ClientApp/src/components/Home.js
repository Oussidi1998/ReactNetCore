import React, { Component } from 'react';
import axios from "axios";
import { Login } from './Login';
import { Redirect } from 'react-router-dom';
import { Button } from 'reactstrap';

export class Home extends Component {

    constructor(props) {
        super(props);
        this.state = { user: {}};
    }

    async componentDidMount() {
        const token = localStorage.getItem("userToken");
        console.log(token);
        const authHeader = {
            headers: {
                Authorization: 'Bearer ' + token,
            },
        };
        try{
            const res = await axios.get('api/users', authHeader);
            if (res.data.username != "") {
                console.log(res.data);
                this.setState({ user: res.data });
            } else {
                this.props.history.push('/login');
            }
        }catch{
            this.props.history.push('/login');
        }
    }

    logout = () => {

        localStorage.removeItem("userToken");
        this.props.history.push("/login");
    }
  render () {
    return (
      <div className="text-center">
            <br />
            <h3>Welcome</h3>
            <h1>Username : {this.state.user.username}</h1>
            <h1>Role : {this.state.user.role}</h1>   
            <br />
            <br />
            <Button onClick={this.logout}>Logout</Button>
        </div>
    );
  }
}
