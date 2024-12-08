import * as React from 'react';
import { Box } from '@mui/material';
import { styled, ThemeProvider, createTheme } from '@mui/material/styles';
import Divider from '@mui/material/Divider';
import { List } from '@mui/material';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import Paper from '@mui/material/Paper';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import { ArrowRight } from '@mui/icons-material';
import ReplayIcon from '@mui/icons-material/Replay';
import { KeyboardArrowDown } from '@mui/icons-material';
import { Home } from '@mui/icons-material';
import { Settings } from '@mui/icons-material';
import { People } from '@mui/icons-material';
import { PermMedia } from '@mui/icons-material';
import { Dns } from '@mui/icons-material';
import Public from '@mui/icons-material/Public';
import { Star } from '@mui/icons-material';
import { Inbox } from '@mui/icons-material';
import ManageAccountsIcon from '@mui/icons-material/ManageAccounts';
import { Equalizer } from '@mui/icons-material';
import { Lightbulb } from '@mui/icons-material';
import { Favorite } from '@mui/icons-material';
import { Circle } from '@mui/icons-material';
import { FiberManualRecord } from '@mui/icons-material';
import Autocomplete from '@mui/material/Autocomplete';

type subjectsType = {
    subjectId: number;
    subjectName: string;
    parentID: number;
    userId: number;
    color: string;
}
type CustomizedListProps = {
    cangeFilter: (newFilter: string) => void;
    subjects: subjectsType[];
    filter: string;
    renderfunc: () => void;
}


// קומפוננטה בשביל הקישורים הימניים של האתר
export default function Navbar(props: CustomizedListProps) {
    const [open, setOpen] = React.useState(false);


    const data2 = [
        { icon: <ManageAccountsIcon />, label: 'ניהול קטגוריות', filter: "$viewSubject" },
        { icon: <Lightbulb />, label: 'אודות', filter: "$about" },

    ];
    const FireNav = styled(List)<{ component?: React.ElementType }>({
        '& .MuiListItemButton-root': {
            paddingLeft: 24,
            paddingRight: 24,
            width: "15.5vw"
        },
        '& .MuiListItemIcon-root': {
            minWidth: 0,
            marginRight: '0vw',
        },
        '& .MuiSvgIcon-root': {
            fontSize: 20,
        },
    });
    return (<>
        <script src="https://cdn.lordicon.com/lusqsztk.js"></script>
        <Box sx={{
            display: 'flex',
            direction: 'rtl',
            //  
            '& .MuiListItemButton-root:hover ': {
                backgroundColor: '#dedede !important',
            },
            '& .MuiTypography-root ': {
                paddingRight: "16px"
            },
            '& .MuiPaper-root': {
                width: '18vw',
                marginRight: "2vw"
            },
            '& .css-ms0i80-MuiTypography-root': {
                textAlign: 'right',
                marginRight: '0vw'
            },
            '& .css-gdmmq3-MuiButtonBase-root-MuiListItemButton-root:hover': {
                margin: '4px',
                backgroundColor: '#dedede',
                borderRadius: '10px',
            },
            '& .MuiListItemButton-root': {
                margin: '4px',
                backgroundColor: '#91a5aa',
                borderRadius: '10px',
                color: '#454545'
            },
            '& .css-hmaglg-MuiPaper-root': {
                backgroundColor: '#fff0',
            },
            '& .css-1mttud9': {
                backgroundColor: '#fff0',
                paddingBottom: "2px"
            },
            '& .css-rr7dug-MuiDivider-root': {
                borderBottomWidth: 'inherit'
            },
            '& .css-jb4snd-MuiTypography-root': {
                textAlign: 'right',
                color: '#454545'
            },
            '& .css-xbjcpz-MuiTypography-root': {
                textAlign: 'right',
                marginRight: '7px',
                color: '#454545'
            },
            '& .css-1tg6gny-MuiButtonBase-root-MuiListItemButton-root:hover': {
                backgroundColor: '#dedede',
            },
            '& .css-1tg6gny-MuiButtonBase-root-MuiListItemButton-root': {
                backgroundColor: '#91a5aa',
                color: '#454545'
            },
            '& .css-m3zjva-MuiSvgIcon-root': {
                color: '#454545'
            },
            '& .css-ail7ou-MuiButtonBase-root-MuiListItemButton-root:hover ': {
                backgroundColor: '#dedede'
            },
            '& .css-o6bm5k-MuiButtonBase-root-MuiIconButton-root svg': {
                color: '#909090',
                fontSize: '29px'
            },
            '& .css-hbegf5-MuiButtonBase-root-MuiListItemButton-root:hover ': {
                backgroundColor: '#dedede'
            },
            '& .css-13cxy3i-MuiListItem-root': {
                marginTop: '6px'
            },
            '& .css-48z56f-MuiTypography-root': {
                fontSize: '20px',
            },
        }}>
            <ThemeProvider
                theme={createTheme({
                    components: {
                        MuiListItemButton: {
                            defaultProps: {
                                disableTouchRipple: true,
                            },
                        },
                    },
                    palette: {
                        mode: 'dark',
                        primary: { main: 'rgb(102, 157, 246)' },
                        background: { paper: 'rgb(5, 30, 52)' },
                    },

                })}
            >
                <Paper elevation={0} sx={{ maxWidth: 256 }}>
                    <FireNav component="nav" disablePadding>
                        <ListItem component="div" disablePadding>
                            <ListItemButton
                                onClick={() => props.cangeFilter("$all")}
                                sx={{ height: 56 }}>
                                <ListItemIcon>
                                    <Inbox color="primary" />
                                </ListItemIcon>
                                <ListItemText
                                    primary="דואר נכנס"
                                    primaryTypographyProps={{
                                        color: 'primary',
                                        fontWeight: 'medium',
                                        variant: 'body2',
                                    }}
                                />
                            </ListItemButton>
                            <Tooltip title="refresh">
                                <IconButton
                                    onClick={props.renderfunc}
                                    size="large"
                                    sx={{
                                        '& svg': {
                                            color: 'grey !important',
                                            transition: '0.2s',
                                            transform: 'translateX(0) rotate(0)',
                                            fontSize: "28px !important"
                                        },
                                        '& .css-h9s4dq-MuiButtonBase-root-MuiIconButton-root': {
                                        },
                                        '&:hover ': {
                                            bgcolor: 'unset',
                                            '& svg:first-of-type': {
                                                color: "grey",
                                                transform: ' rotate(-80deg)',
                                            },
                                            '& svg:last-of-type': {
                                                right: 0,
                                                opacity: 1,
                                            },
                                        },
                                        '&:after': {
                                            content: '""',
                                            position: 'absolute',
                                            height: '80%',
                                            display: 'block',
                                            left: 0,
                                            width: '1px',
                                            bgcolor: 'divider',
                                        },
                                    }}
                                >
                                    <ReplayIcon />
                                </IconButton>
                            </Tooltip>
                        </ListItem>
                        <Divider />
                        <Box
                            sx={{
                                bgcolor: open ? 'rgba(71, 98, 130, 0.2)' : null,
                                pb: open ? 2 : 0,
                            }}
                        >
                            <ListItemButton
                                alignItems="flex-start"
                                onClick={() => setOpen(!open)}
                                sx={{
                                    px: 3,
                                    pt: 2.5,
                                    pb: open ? 0 : 2.5,
                                    '&:hover, &:focus': { '& svg': { opacity: open ? 1 : 1 } },
                                }}
                            >
                                <ListItemText

                                    primary="◉ קטגוריות"
                                    primaryTypographyProps={{
                                        fontSize: 15,
                                        textAlign: 'right',
                                        fontWeight: 'medium',
                                        lineHeight: '20px',
                                        mb: '2px',
                                    }}
                                    secondary="דואר נכנס מסווג לפי נושאים"
                                    secondaryTypographyProps={{
                                        noWrap: true,
                                        fontSize: 12,
                                        lineHeight: '16px',
                                        color: open ? 'rgba(0,0,0,0)' : 'rgba(255,255,255,0.5)',
                                    }}
                                    sx={{ my: 0 }}
                                />
                                <KeyboardArrowDown
                                    sx={{
                                        mr: -1,
                                        opacity: 1,
                                        transform: open ? 'rotate(-180deg)' : 'rotate(0)',
                                        transition: '0.2s',
                                    }}
                                />
                            </ListItemButton>
                            {open &&
                                Object.values(props.subjects).map((item) => (
                                    <ListItemButton
                                        onClick={() => props.cangeFilter(item.subjectName)}
                                        key={item.subjectName}
                                        sx={{ py: 0, minHeight: 32, color: 'rgba(255,255,255,.8)' }}
                                        style={{ backgroundColor: item.subjectName == props.filter ? "#dedede" : "#91a5aa" }}
                                    >
                                        <ListItemIcon sx={{ color: 'inherit', paddingRight: '30px' }}>
                                            <FiberManualRecord sx={{ color: item.color, stroke: "#454545", strokeWidth: 1 }} />
                                        </ListItemIcon>
                                        <ListItemText
                                            primary={item.subjectName}
                                            primaryTypographyProps={{ fontSize: 14, fontWeight: 'medium' }}
                                        />
                                    </ListItemButton>
                                ))}
                        </Box>
                        <Box
                            sx={{
                                bgcolor: '#fff0',
                                pb: 2,
                            }}
                        >
                            {data2.map((item) => (
                                <ListItemButton
                                    key={item.label}
                                    sx={{ py: 0, minHeight: 32, color: 'rgba(255,255,255,.8)' }}
                                    onClick={() => props.cangeFilter(item.filter)}
                                    style={{ backgroundColor: item.filter == props.filter ? "#dedede" : "#91a5aa" }}
                                >
                                    <ListItemIcon sx={{ color: 'inherit' }}>
                                        {item.icon}

                                    </ListItemIcon>
                                    <ListItemText
                                        primary={item.label}
                                        primaryTypographyProps={{ fontSize: 14, fontWeight: 'medium' }}
                                    />
                                </ListItemButton>
                            ))}
                        </Box>
                    </FireNav>
                </Paper>
            </ThemeProvider>
        </Box></>
    );
}