import * as React from 'react';
import { connect } from 'react-redux'
import { login, logout } from '../store/actions/user'
import { Avatar } from '@mui/material';
import { Button } from '@mui/material';
import { CssBaseline } from '@mui/material';
import { TextField } from '@mui/material';
import { FormControlLabel } from '@mui/material';
import { Checkbox } from '@mui/material';
import { Link } from '@mui/material';
import { Grid } from '@mui/material';
import { Box } from '@mui/material';
//import {LockOutlinedIcon} from '@mui/icons-material';
import { Typography } from '@mui/material';
import { Container } from '@mui/material';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { useNavigate } from 'react-router-dom';
import axios from "axios";
import { gridSelectionStateSelector } from '@mui/x-data-grid';
import swal from 'sweetalert';
import { LinearProgress } from '@mui/material';
import Autocomplete from '@mui/material/Autocomplete';
import emailjs, { init } from "@emailjs/browser";
import startPic from "../startPic.png";

const theme = createTheme();

function SignIn(props: { in: (arg0: { email: string; password: string; }) => void; }) {
    const navigate = useNavigate();
    const [thinking, setThinking] = React.useState(false);
    React.useEffect(() => {
        const connection = async () => {
            const response = await axios.post("https://localhost:44391/api/User/CheckConection");
            console.log(response.data)
        }
        connection();
    }, []);

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        setThinking(true);

        const data = new FormData(event.currentTarget);
        const email = String(data.get('Address'));
        const password = String(data.get('password'));


        let re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (re.test(email)) {
            if (password !== "") {
                const user = { mail: email, userPassword: password }
                console.log(user);

                const response = await axios.post("https://localhost:44391/api/User/Login", user);
                if (response.data.User != null) {
                    props.in({ email: email, password: password + "" });
                    localStorage.setItem('mail', email);
                    localStorage.setItem('password', password);
                    navigate('/home');
                }
                else {
                    if (response.data.ExistMail == true) {
                        swal("!סיסמא שגויה", "אנא נסה שנית", "error");
                        setThinking(false);
                    }
                    else
                        swal({
                            title: "!משתמש אינו קיים",
                            text: "?מעוניין ליצור חשבון חדש אצלנו",
                            icon: "info",
                            dangerMode: true,
                            buttons: [true, true]
                        }).then((confirm) => {
                            if (confirm) {
                                init("YOUR_INIT_PASS_FOR_THE_EMAIL-JD_ACC");
                                const serviceID = 'YOUR_SERVICE_ID';
                                const templateID = 'YOUR_TEMPLATE_ID';
                                const code = Math.floor(Math.random() * 1000000);
                                emailjs.send(serviceID, templateID, { Address: email, toName: email.split('@')[0], message: code })
                                    .then(() => {
                                        swal(":הזינו את הקוד האישי שנשלח אליכם למייל", {
                                            content: {
                                                element: "input"
                                            },
                                        })
                                            .then(async (value) => {
                                                if (value == code) {
                                                    axios.post("https://localhost:44391/api/User", user);
                                                    props.in({ email: email, password: password + "" });
                                                    localStorage.setItem('mail', email);
                                                    localStorage.setItem('password', password);
                                                    await swal("!הפרטים נשמרו בהצלחה", {
                                                        icon: "success",
                                                    });
                                                    navigate('/home');
                                                }
                                                else {
                                                    swal("!סיסמא שגויה", "אנא נסה שנית", "error");
                                                    setThinking(false);

                                                }
                                            });
                                    },
                                        (err) => {
                                            alert(JSON.stringify(err));
                                            setThinking(false);
                                        });
                            } else {
                                setThinking(false);
                            }
                        });


                }

            } else {
                swal("!לא בחרת סיסמה", "אנא נסה שנית", "error");
                setThinking(false);
            }

        } else {
            swal("!מייל לא תקין", "אנא נסה שנית", "error");
            setThinking(false);
        }

    };

    return (
        <Box display="grid" gridTemplateColumns="repeat(12, 1fr)" gap={1}>
            <Box gridColumn="span 12" >
                <ThemeProvider theme={theme} >
                    <Container component="main" maxWidth="xs" style={{
                        height: "0vh",
                    }}>
                        <CssBaseline />
                        <Box

                            sx={{
                                display: 'flex',
                                flexDirection: 'column',
                                alignItems: 'center',
                                marginTop: "110px"
                            }}
                        >
                            <Avatar sx={{ m: 1, bgcolor: '#643695' }}>
                            </Avatar>
                            <Typography component="h1" variant="h5">
                                Sign In
                            </Typography>
                            <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
                                {<TextField
                                    placeholder=""
                                    margin="normal"
                                    fullWidth
                                    id="email"
                                    label="כתובת מייל"
                                    name="Address"
                                    autoComplete="email"
                                    autoFocus
                                    dir="ltr"

                                />}
                                {<TextField
                                    placeholder=""
                                    margin="normal"
                                    fullWidth
                                    name="password"
                                    label="סיסמא"
                                    type="password"
                                    id="password"
                                    autoComplete="current-password"
                                />}
                                <FormControlLabel
                                    control={<Checkbox value="remember" color="primary" />}
                                    label="Remember me"
                                />
                                <Button
                                    type="submit"
                                    fullWidth
                                    disabled={thinking}
                                    variant="contained"
                                    sx={{ mt: 3, mb: 2, backgroundImage: "linear-gradient(to right,#f8646e, #763e94 , #16169a)" }}

                                >
                                    כניסה
                                </Button>

                            </Box>
                        </Box>
                        {thinking && <Box sx={{ width: '100%' }}>
                            <LinearProgress />
                        </Box>}

                    </Container>
                </ThemeProvider>
            </Box>
            <Box gridColumn="span 12" >
                <img src={startPic} width="1200vw" style={{ width: "-webkit-fill-available !important" }}></img>
            </Box>
        </Box>

    );
}

const mapDispatchToProps = {
    in: login, // הפעולה מאקשין
    out: logout
};
// הקומפוננטה הזו אמורה לשנות ערכים בסטייט
// connect זה הפרמטר השני שמקבלת הפונקציה
// null אם לא רוצים לקבל מהסטייט שולחים
export default connect(null, mapDispatchToProps)(SignIn)

