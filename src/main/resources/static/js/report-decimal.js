/* Exact base-ten sums for SQL decimal strings; no binary floating-point aggregation. */
'use strict';
window.ReportDecimal = (() => {
    function parse(value) {
        const text = String(value ?? '0').trim();
        const match = /^([+-]?)(\d+)(?:\.(\d*))?(?:[eE]([+-]?\d+))?$/.exec(text);
        if (!match) throw new Error('Invalid report decimal: ' + text);
        const exponent=Number(match[4]||0);
        if(!Number.isSafeInteger(exponent)||Math.abs(exponent)>1000)throw new Error('Invalid report decimal exponent: '+text);
        let units=BigInt(match[2]+(match[3]||''))*(match[1]==='-'?-1n:1n),scale=(match[3]||'').length-exponent;
        if(scale<0){units*=10n**BigInt(-scale);scale=0;}
        return {units,scale};
    }
    function text(units, scale) {
        const sign = units < 0n ? '-' : '';
        const digits = (units < 0n ? -units : units).toString().padStart(scale + 1, '0');
        return sign + (scale ? digits.slice(0, -scale) + '.' + digits.slice(-scale) : digits);
    }
    function sum(values) {
        let units = 0n, scale = 0;
        for (const value of values) {
            const part = parse(value);
            const next = Math.max(scale, part.scale);
            units = units * 10n ** BigInt(next - scale) + part.units * 10n ** BigInt(next - part.scale);
            scale = next;
        }
        return text(units, scale);
    }
    function format(value, places = 0, parentheses = false) {
        let { units, scale } = parse(value);
        const negative = units < 0n;
        if (negative) units = -units;
        if (scale > places) {
            const divisor = 10n ** BigInt(scale - places);
            units = (units + divisor / 2n) / divisor;
        } else units *= 10n ** BigInt(places - scale);
        if (parentheses && units === 0n) return '0';
        const [whole, fraction] = text(units, places).split('.');
        const formatted = whole.replace(/\B(?=(\d{3})+(?!\d))/g, ',') + (fraction === undefined ? '' : '.' + fraction);
        return negative && units !== 0n ? parentheses ? '(' + formatted + ')' : '-' + formatted : formatted;
    }
    return { sum, format };
})();
