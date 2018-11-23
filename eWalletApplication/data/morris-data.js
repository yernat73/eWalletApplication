$(function() {

    Morris.Area({
        element: 'morris-area-chart',
        data: [{
            period: '2010 Q1',
            Food: 2666,
            Housing: null,
            Roads: 2647
        }, {
            period: '2010 Q2',
            Food: 2778,
            Housing: 2294,
            Roads: 2441
        }, {
            period: '2010 Q3',
            Food: 4912,
            Housing: 1969,
            Roads: 2501
        }, {
            period: '2010 Q4',
            Food: 3767,
            Housing: 3597,
            Roads: 5689
        }, {
            period: '2011 Q1',
            Food: 6810,
            Housing: 1914,
            Roads: 2293
        }, {
            period: '2011 Q2',
            Food: 5670,
            Housing: 4293,
            Roads: 1881
        }, {
            period: '2011 Q3',
            Food: 4820,
            Housing: 3795,
            Roads: 1588
        }, {
            period: '2011 Q4',
            Food: 15073,
            Housing: 5967,
            Roads: 5175
        }, {
            period: '2012 Q1',
            Food: 10687,
            Housing: 4460,
            Roads: 2028
        }, {
            period: '2012 Q2',
            Food: 8432,
            Housing: 5713,
            Roads: 1791
        }],
        xkey: 'period',
        ykeys: ['Food', 'Housing', 'Roads'],
        labels: ['Food', 'Housing', 'Roads'],
        pointSize: 2,
        hideHover: 'auto',
        resize: true
    });
    
    
});
