import React, { Component } from 'react';
import { Container, Row, Col, Button, Alert } from 'reactstrap';
import axios from "axios";

export class Login extends Component {
  constructor(props) {
    super(props);
    this.state = { username:"" ,password:"",loginSuccess:true};
  }

    handleFormChange = (e) => {
        this.setState({
            [e.target.name]: e.target.value
        })
    }

    SubmitLogin = async (e) => {

        await axios.post('api/users', JSON.stringify(this.state), {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        }).then(res => {
            console.log(res.data)
            if (res.data) {
                localStorage.setItem("userToken", res.data);
                this.props.history.push("/home");
            }
        }).catch(err => {
            this.setState({
                loginSuccess: false,
            })
        });        
    }

    componentDidMount() {
        const token = localStorage.getItem("userToken");
        if (token != null && token.length > 0) {
            this.props.history.push("/home");
        }
    }

    render() {
        const { username, password, loginSuccess } = this.state;
      return (
          <Container>
              <Row className="justify-content-center mt-5">
                  <Col md="5">
                      <h2 className="text-center">Se Connecter</h2> <br /> <br />
                      {
                          !loginSuccess ? (<><Alert color="danger">
                              Invalid username or password
                                </Alert> <br /></>) : null
                      }
                      <form className="text-center">
                          <input className="form-control" placeholder="username" value={username} onChange={this.handleFormChange} type="text" name="username" />
                          <br/>    
                          <input className="form-control" placeholder="password" value={password} onChange={this.handleFormChange} type="text" name="password" />
                          <br />
                          <Button color="primary" className="form-control" disabled={!username || !password} onClick={this.SubmitLogin} > Log in</Button>    
                      </form>
                  </Col>
              </Row>
          </Container>
    );
  }
}
